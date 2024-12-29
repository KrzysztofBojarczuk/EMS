import axios from "axios";

import { UserProfileToken } from "../Models/User";

const api = "http://localhost:7256/api/";

export const loginAPI = async (username: string, password: string) => {
  const data = await axios.post<UserProfileToken>(api + "account/login", {
    username: username,
    password: password,
  });
  return data;
};

export const registerAPI = async (
  email: string,
  username: string,
  password: string
) => {
  const data = await axios.post<UserProfileToken>(api + "account/register", {
    email: email,
    username: username,
    password: password,
  });
  return data;
};
