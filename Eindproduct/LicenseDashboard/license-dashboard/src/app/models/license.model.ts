export class License {
  public company: string;
  public licenseAmount: number;
  public lastPayment: Date;
  public expiryDate: Date;
  public paymentCompleted: boolean;

  constructor(
    company: string,
    licenseAmount: number,
    lastPayment: Date,
    expiryDate: Date,
    paymentCompleted: boolean
  ) {
    this.company = company;
    this.licenseAmount = licenseAmount;
    this.lastPayment = lastPayment;
    this.expiryDate = expiryDate;
    this.paymentCompleted = paymentCompleted;
  }
}
