export interface AddressGet {
  id: string;
  city: string;
  street: string;
  number: string;
  zipCode: string;
}

export interface AddressPost {
  city: string;
  street: string;
  number: string;
  zipCode: string;
}
