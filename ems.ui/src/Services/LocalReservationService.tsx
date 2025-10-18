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
  const response = await axios.post<LocalPost>(api + "Local", localPost);
  return response.data;
};

export const UserPostReservationService = async (
  reservationPost: ReservationPost
) => {
  const response = await axios.post<ReservationPost>(
    api + "Reservation",
    reservationPost
  );
  return response.data;
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
  return response.data;
};

export const DeleteLocalService = async (id: string) => {
  const response = await axios.delete(`${api}Local/${id}`);
  return response.data;
};

export const UserUpdateLocalService = async (
  localPost: LocalPost,
  id: string
) => {
  const response = await axios.put<LocalPost>(`${api}Local/${id}`, localPost);
  return response.data;
};
