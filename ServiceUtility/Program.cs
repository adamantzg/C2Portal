using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ExcelDataReader;
using NLog;
using Portal.Model;
using ServiceUtility.Model;

namespace ServiceUtility
{
	class Program
	{
		static Logger Nlog = LogManager.GetCurrentClassLogger();

		static void Main(string[] args)
		{
			var parameters = ParseArguments(args);
			if (parameters.ContainsKey("/t"))
			{
				if (parameters["/t"] == "EXPORTSALESDATA")
				{
					DateTime? from = GetMonthStart(ProcessDateParameter(parameters, "/from", DateTime.Now));
					ExportSalesData(from);
				}
				else if (parameters["/t"] == "IMPORTDATA")
				{
					if (!parameters.ContainsKey("/f"))
					{
						Console.WriteLine("/f parameter not specified");
						return;
					}

					var folder = parameters["/f"];

					ImportData(folder);
				}
			}
		}

		private static DateTime GetMonthStart(DateTime date)
		{
			return date.Date.AddDays(-1 * date.Day + 1);
		}

		private static void ImportData(string folder)
		{
			var customerFile = Path.Combine(folder, "customers.xls");

			var unitOfWork = new UnitOfWork();
			if (File.Exists(customerFile))
			{
				var customers = unitOfWork.CustomerRepository.Get().ToList();
				var newCustomers = new List<Customer>();

				using (var stream = File.Open(customerFile, FileMode.Open, FileAccess.Read))
				{
					using (var reader = ExcelReaderFactory.CreateReader(stream))
					{
						do
						{
							reader.Read(); //skip header
							while (reader.Read())
							{
								var importedCustomer = ImportCustomerRow(reader);
								var customer = customers.FirstOrDefault(c => c.code == importedCustomer.code);
								if (customer == null)
								{
									newCustomers.Add(importedCustomer);
								}
								else
								{
									customer.name = importedCustomer.name;
									customer.address1 = importedCustomer.address1;
									customer.address2 = importedCustomer.address2;
									customer.address3 = importedCustomer.address3;
									customer.address4 = importedCustomer.address4;
									customer.address5 = importedCustomer.address5;
									customer.address6 = importedCustomer.address6;
									customer.town_city = importedCustomer.town_city;
									customer.county = importedCustomer.county;
									customer.currency = importedCustomer.currency;
									customer.invoice_customer = importedCustomer.invoice_customer;
									customer.analysis_codes_1 = importedCustomer.analysis_codes_1;
								}
								Console.WriteLine("Importing customer {0}", importedCustomer.code);
							}
						} while (reader.NextResult());

						unitOfWork.Save();
						//Save new
						unitOfWork.CustomerRepository.BulkInsert(newCustomers);
						unitOfWork.Save();
					}
				}
			}

			var productFile = Path.Combine(folder, "products.xls");
			unitOfWork = new UnitOfWork();
			if (File.Exists(productFile))
			{
				var products = unitOfWork.ProductRepository.Get().ToList();
				var newProducts = new List<Product>();

				using (var stream = File.Open(productFile, FileMode.Open, FileAccess.Read))
				{
					using (var reader = ExcelReaderFactory.CreateReader(stream))
					{
						do
						{
							reader.Read(); //skip header
							while (reader.Read())
							{
								var importedProduct = ImportProductRow(reader);
								var product = products.FirstOrDefault(c => c.code == importedProduct.code);
								if (product == null)
								{
									newProducts.Add(importedProduct);
								}
								else
								{
									product.name = importedProduct.name;
									product.description = importedProduct.description;
								
								}
								Console.WriteLine("Importing product {0}", importedProduct.code);
							}
						} while (reader.NextResult());

						unitOfWork.Save();
						//Save new
						unitOfWork.ProductRepository.BulkInsert(newProducts);
						unitOfWork.Save();
					}
				}
			}
		}

		private static Customer ImportCustomerRow(IExcelDataReader reader)
		{
			var c = new Customer();
			c.code = GetStringOrNull(reader.GetValue(0));
			c.name = GetStringOrNull(reader.GetValue(1));
			c.address1 = GetStringOrNull(reader.GetValue(2));
			c.address2 = GetStringOrNull(reader.GetValue(3));
			c.address3 = GetStringOrNull(reader.GetValue(4));
			c.address4 = GetStringOrNull(reader.GetValue(5));
			c.town_city = GetStringOrNull(reader.GetValue(6));
			c.county = GetStringOrNull(reader.GetValue(7));
			c.address6 = GetStringOrNull(reader.GetValue(8));
			c.address5 = GetStringOrNull(reader.GetValue(9));
			c.analysis_codes_1 = GetStringOrNull(reader.GetValue(10));
			c.currency = GetStringOrNull(reader.GetValue(11));
			c.invoice_customer = GetStringOrNull(reader.GetValue(12));
			return c;
		}

		private static Product ImportProductRow(IExcelDataReader reader)
		{
			var p = new Product();
			p.code = GetStringOrNull(reader.GetValue(1));
			p.name = GetStringOrNull(reader.GetValue(2));
			p.description = GetStringOrNull(reader.GetValue(3));
			return p;
		}

		private static void ExportSalesData(DateTime? from)
		{
			try
			{
				var orderData = DAL.GetOrders(from);
				var sales_data = new List<sku_sales_data>();
				foreach (var order in orderData)
				{
					var i = 0;
					while (i < order.Lines.Count)
					{
						var line = order.Lines[i];
						int j;
						double? total_cost = line.cost_of_sale, total_price = line.val;
						switch (line.line_type)
						{
							case "B":
								j = i + 1;
								sales_data.Add(new sku_sales_data { rowid = line.rowid, line_type = "B", cost = total_cost, price = total_price });
								while (j < order.Lines.Count && order.Lines[j].line_type == "P")
								{
									line = order.Lines[j];
									sales_data.Add(new sku_sales_data { rowid = line.rowid, line_type = "BP", cost = line.cost_of_sale, price = (total_cost != 0 ? line.cost_of_sale / total_cost * total_price : 0) });
									j++;
								}
								i = j;
								break;
							case "P":
							case "C":
								sales_data.Add(new sku_sales_data { rowid = line.rowid, line_type = line.line_type, cost = line.cost_of_sale, price = line.val });
								i++;
								break;
							case "S":
								if (string.IsNullOrEmpty(line.product_group_a))
									sales_data.Add(new sku_sales_data { rowid = line.rowid, line_type = "P", cost = (line.val > 0 ? 1 : -1) * line.cost_of_sale, price = line.val });
								else
								{
									j = i + 1;
									var serviceData = new List<sku_sales_data>();
									serviceData.Add(new sku_sales_data { rowid = line.rowid, line_type = "S", cost = total_cost, price = total_price });

									while (j < order.Lines.Count && order.Lines[j].line_type == "P" && order.Lines[j].product.Trim() != "L")
									{
										line = order.Lines[j];
										serviceData.Add(new sku_sales_data { rowid = line.rowid, line_type = "SP", cost = line.cost_of_sale, price = 0 });
										j++;
									}
									serviceData[0].cost = serviceData.Where(d => d.line_type == "SP").Sum(d => d.cost);
									foreach (var sd in serviceData.Where(d => d.line_type == "SP"))
									{
										sd.price = serviceData[0].cost != 0 ? sd.cost / serviceData[0].cost * serviceData[0].price : 0;
									}
									sales_data.AddRange(serviceData);

									i = j;
								}
								break;
						}

					}
				}
				DAL.BulkInsertSalesData(sales_data);
			}
			catch (Exception ex)
			{
				Nlog.Log(LogLevel.Error, ex.Message);
				throw;
			}

		}

		private static Dictionary<string, string> ParseArguments(string[] args)
		{
			var result = new Dictionary<string, string>();
			foreach (var item in args)
			{
				if (!item.StartsWith("/"))
					throw new ArgumentException($"Incorrect argument {item}");
				var parts = item.Split(new char[] { ':' }, 2);
				var value = string.Empty;
				if (parts.Length > 1)
					value = parts[1];
				result.Add(parts[0].ToLower(), value);
			}
			return result;
		}

		private static DateTime ProcessDateParameter(Dictionary<string, string> parameters, string param, DateTime? defaultDate = null)
		{
			if (defaultDate == null)
				defaultDate = DateTime.Now;
			if (!parameters.ContainsKey(param))
				return defaultDate.Value;
			var sParam = parameters[param];
			DateTime result;
			if (sParam.StartsWith("T"))
			{
				var offset = int.Parse(sParam.Substring(1));
				result = DateTime.Now.AddDays(offset);
			}
			else
				result = DateTime.Parse(sParam);
			return result;
		}

		public static T? FromDbValue<T>(object value) where T: struct
		{
			if (value == null || value == DBNull.Value)
				return null;
			else
			{
				//try
				//{
				TypeConverter t = TypeDescriptor.GetConverter(typeof(T));
				if (value.GetType() != typeof(T))
				{
					return (T)t.ConvertTo(value, typeof(T));
				}
				return (T)value;
				//}
				//catch (Exception ex)
				//{
				//    throw;
				//}
			}
		}

		public static string GetStringOrNull(object value)
		{
			if (value == null || value == DBNull.Value)
				return null;
			return string.Empty + value;
		}
	}
}
