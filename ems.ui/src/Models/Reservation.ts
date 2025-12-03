export interface ReservationGet {
  id: string;
  description: string;
  checkInDate: string;
  checkOutDate: string;
}

export interface ReservationPost {
  localId: string;
  description: string;
  checkInDate: string;
  checkOutDate: string;
}

export interface PaginatedReservationResponse {
  reservationGet: ReservationGet[];
  totalItems: number;
  totalPages: number;
  pageIndex: number;
}
