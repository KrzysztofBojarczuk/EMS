export interface ReservationGet {
  id: string;
  checkInDate: string;
  checkOutDate: string;
}

export interface ReservationPost {
  localId: string;
  checkInDate: string;
  checkOutDate: string;
}

export interface PaginatedReservationResponse {
  reservationGet: ReservationGet[];
  totalItems: number;
  totalPages: number;
  pageIndex: number;
}
