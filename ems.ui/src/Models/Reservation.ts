export interface ReservationGet {
  id: string;
  localId: string;
  checkInDate: string;
  checkOutDate: string;
}

export interface ReservationPost {
  localId: string;
  checkInDate: string;
  checkOutDate: string;
}

export interface PaginatedReservationResponse {
  localGet: ReservationGet[];
  totalItems: number;
  totalPages: number;
  pageIndex: number;
}
