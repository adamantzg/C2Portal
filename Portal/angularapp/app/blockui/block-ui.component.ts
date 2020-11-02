import { Component, OnInit } from '@angular/core';
import { BlockUIService } from '../../common_components/services/block-ui.service';

@Component({
  selector: 'app-block-ui',
  templateUrl: './block-ui.component.html',
  styleUrls: ['./block-ui.component.css']
})
export class BlockUiComponent implements OnInit {

  constructor(private blockUIService: BlockUIService) { }
  blockUI = false;

  ngOnInit() {
    this.blockUIService.blockUIEvent.subscribe( data => this.blockUI = data);
  }

}
