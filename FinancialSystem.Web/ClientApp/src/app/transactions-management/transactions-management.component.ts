import { Component, Inject, OnInit, ViewChild } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { HttpClient } from '@angular/common/http';

import { ToastrService } from 'ngx-toastr';
import { TransactionsTableComponent } from '../transactions-table/transactions-table.component';

@Component({
  selector: 'app-transactions-management',
  templateUrl: './transactions-management.component.html',
  styleUrls: ['./transactions-management.component.css']
})
export class TransactionsManagementComponent implements OnInit {
  @ViewChild('transactionsTable') transactionsTable: TransactionsTableComponent;
  public addTransaction: FormGroup;

  constructor(private fb: FormBuilder,
    private http: HttpClient,
    private toastr: ToastrService,
    @Inject('BASE_URL') private baseUrl: string) { }

  ngOnInit() {
    this.createForm();
  }

  onSubmit() {
    let request = {
      type:  parseInt(this.addTransaction.controls.type.value),
      amount: parseFloat(this.addTransaction.controls.amount.value)
    };

    this.http.post(this.baseUrl + 'api/transactions', request).subscribe(
      successResponse => {
        this.addTransaction.reset();
        this.transactionsTable.reloadData();
        this.toastr.success('Transaction was successfully completed!');
      },
      errorResponse => {
        this.toastr.error(JSON.stringify(errorResponse.error));
      },
      () => {});
  }

  private createForm(): void {
    this.addTransaction = this.fb.group({
      type: ['', [Validators.required]],
      amount: ['', [Validators.required, Validators.min(1)]],
    });
  }

}
