import { createContext, useEffect, useState } from "react";
import { UserProfile } from "../Models/User";
import { useNavigate } from "react-router-dom";
import React from "react";
import axios from "axios";
import { loginAPI, registerAPI } from "../Services/AuthService";

type UserContextType = {
  user: UserProfile | null;
  token: string | null;
  registerUser: (email: string, username: string, password: string) => void;
  loginUser: (username: string, password: string) => void;
  logout: () => void;
  isLoggedIn: () => boolean;
};

type Props = { children: React.ReactNode };

const UserContext = createContext<UserContextType>({} as UserContextType);

export const UserProvider = ({ children }: Props) => {
  const navigate = useNavigate();
  const [token, setToken] = useState<string | null>(null);
  const [user, setUser] = useState<UserProfile | null>(null);
  const [isReady, setIsReady] = useState(false);

  useEffect(() => {
    const user = localStorage.getItem("user");
    const token = localStorage.getItem("token");

    if (user && token) {
      setUser(JSON.parse(user));
      setToken(token);
      axios.defaults.headers.common["Authorization"] = "Bearer " + token;
    }

    setIsReady(true);
  }, []);

  const registerUser = async (
    email: string,
    username: string,
    password: string
  ) => {
    await registerAPI(email, username, password).then((res) => {
      if (res) {
        localStorage.setItem("token", res?.data.token);
        const userObj = {
          userName: res?.data.userName,
          email: res?.data.email,
          roles: res?.data.roles,
        };

        localStorage.setItem("user", JSON.stringify(userObj));

        setToken(res?.data.token!);
        setUser(userObj!);
        navigate("/login");
      }
    });
  };

  const loginUser = async (username: string, password: string) => {
    try {
      const res = await loginAPI(username, password);
      localStorage.setItem("token", res?.data.token);
      const userObj = {
        userName: res?.data.userName,
        email: res?.data.email,
        roles: res?.data.roles,
      };

      localStorage.setItem("user", JSON.stringify(userObj));

      setToken(res?.data.token!);
      setUser(userObj!);
      const token = localStorage.getItem("token");
      axios.defaults.headers.common["Authorization"] = "Bearer " + token;

      if (userObj?.roles.includes("User")) {
        navigate("/Task");
      } else if (userObj?.roles.includes("Admin")) {
        navigate("/Admin");
      } else {
        navigate("/");
      }
    } catch (error: any) {
      if (error.response && error.response.status === 401) {
        throw new Error("Invalid username or password");
      } else {
        throw new Error("An unexpected error occurred");
      }
    }
  };

  const isLoggedIn = () => {
    return !!user;
  };

  const logout = () => {
    localStorage.removeItem("token");
    localStorage.removeItem("user");
    setUser(null);
    setToken("");
    navigate("/");
  };

  return (
    <UserContext.Provider
      value={{ loginUser, user, token, logout, isLoggedIn, registerUser }}
    >
      {isReady ? children : null}
    </UserContext.Provider>
  );
};

export const useAuth = () => React.useContext(UserContext);
