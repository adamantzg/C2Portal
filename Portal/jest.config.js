module.exports = {
    preset: "jest-preset-angular",
    roots: ['<rootDir>/angularapp'],
    "transform": {
      "^.+\\.(ts|html)$": "<rootDir>/node_modules/jest-preset-angular/preprocessor.js",
      "^.+\\.js$": "babel-jest"
    },
    "transformIgnorePatterns": ["node_modules/(?!(ngx-bootstrap))"],
    moduleDirectories: ['<rootDir>/node_modules'],    
    setupTestFrameworkScriptFile: "<rootDir>/angularapp/app/setup-jest.ts",
    "globals": {
      "ts-jest": {
         "tsConfigFile": "<rootDir>/angularapp/tsconfig.spec.json",
         "babelConfig": {
          "presets": ["env"]
        }
       },
       "__TRANSFORM_HTML__": true
     },
     "moduleNameMapper": {
      "^src/(.*)": "<rootDir>/angularapp/app/$1",
      "^app/(.*)": "<rootDir>/angularapp/app/$1",
      "^common_components/(.*)": "<rootDir>/angularapp/common_components/$1",
      "^@crosswater-template/(.*)": "<rootDir>/angularapp/cw-flex/$1",
      "^@crosswater-api/(.*)": "<rootDir>/angularapp/cw-flex/users/$1",      
      "^assets/(.*)": "<rootDir>/angularapp/assets/$1",
      "^environments/(.*)": "<rootDir>/angularapp/environments/$1"
    },
  }