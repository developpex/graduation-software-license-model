export class License {
  public company: string;
  public licenseAmount: number;
  public lastPayment: Date;
  public expiryDate: Date;

  constructor(
    company: string,
    licenseAmount: number,
    lastPayment: Date,
    expiryDate: Date
  ) {
    this.company = company;
    this.licenseAmount = licenseAmount;
    this.lastPayment = lastPayment;
    this.expiryDate = expiryDate;
  }
}
