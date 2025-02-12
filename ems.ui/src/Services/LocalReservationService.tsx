import { LocalPost, PaginatedLocalResponse } from "../Models/Local";
import axios from "axios";
import {
  PaginatedReservationResponse,
  ReservationPost,
} from "../Models/Reservation";

const api = "https://localhost:7256/api/";

export const UserGetLocalService = async (
  pageNumber: number,
  pageSize: number,
  searchTerm: string
) => {
  const response = await axios.get<PaginatedLocalResponse>(api + "Local", {
    params: { pageNumber, pageSize, searchTerm },
  });

  return response.data;
};

export const UserPostLocalService = async (localPost: LocalPost) => {
  return await axios.post<LocalPost>(api + "Local", localPost);
};

export const UserPostReservationService = async (
  reservationPost: ReservationPost
) => {
  return await axios.post<ReservationPost>(
    api + "Reservation",
    reservationPost
  );
};

export const UserGetReservationService = async (
  pageNumber: number,
  pageSize: number,
  searchTerm: string
) => {
  const response = await axios.get<PaginatedReservationResponse>(
    api + "Reservation",
    {
      params: { pageNumber, pageSize, searchTerm },
    }
  );

  return response.data;
};

export const DeleteReservationService = async (id: string) => {
  const response = await axios.delete(`${api}Reservation/${id}`);
  return response;
};

export const DeleteLocalService = async (id: string) => {
  const response = await axios.delete(`${api}Local/${id}`);
  return response;
};
