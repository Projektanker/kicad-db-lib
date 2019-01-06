import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { Location } from '@angular/common';
import { PartService } from '../part.service';
import { Part } from '../part/part';

@Component({
  selector: 'app-part-detail',
  templateUrl: './part-detail.component.html',
  styleUrls: ['./part-detail.component.css']
})
export class PartDetailComponent implements OnInit {
  part: Part;

  partForm = this.fb.group({
    id: [{ value: null, disabled: true }],
    reference: [null], // , Validators.required],
    value: [null], // , Validators.required],
    footprint: [null], // , Validators.required],
    symbol: [null], // , Validators.required],
    library: [null], // , Validators.required],
    datasheet: null,
    description: [null], // , Validators.required],
    keywords: [null], // , Validators.required]
  });

  constructor(
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private partService: PartService,
    private location: Location) { }

  ngOnInit() {
    this.getPart();
    this.part = {
      id: 11, reference: 'R',
      value: '1K',
      footprint: 'Resistor_SMD:R_0603_1608Metric',
      library: 'R_0603',
      description: 'Resistor 1K 0603 75V',
      keywords: 'Res Resistor 1K 0603',
      symbol: '{lib}:R',
      datasheet: 'no datasheet'
    };
    this.partForm.patchValue(this.part);
  }

  getPart(): void {
    const id = +this.route.snapshot.paramMap.get('id');
    this.partService.getPart(id)
      .subscribe((newPart: Part) => {
        this.part = newPart;
        this.partForm.patchValue(this.part);
      });
  }

  onSubmit(): void {
    console.warn(this.partForm.value);
    this.part = this.partForm.value;
    this.partService.updatePart(this.part)
      .subscribe(() => this.goBack());
  }

  goBack(): void {
    this.location.back();
  }
}
