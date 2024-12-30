import React from "react";
import * as Yup from "yup";
import { yupResolver } from "@hookform/resolvers/yup";
import { useForm } from "react-hook-form";
import { useAuth } from "../../../Context/useAuth.tsx";
import { InputText } from "primereact/inputtext";
import { Button } from "primereact/button";
import { Card } from "primereact/card";
import "./LoginPage.css";

type Props = {};

type LoginFormsInputs = {
  userName: string;
  password: string;
};

const validation = Yup.object().shape({
  userName: Yup.string().required("Username is required"),
  password: Yup.string().required("Password is required"),
});

const LoginPage = (props: Props) => {
  const { loginUser } = useAuth();
  const {
    register,
    handleSubmit,
    formState: { errors },
  } = useForm<LoginFormsInputs>({ resolver: yupResolver(validation) });

  const handleLogin = (form: LoginFormsInputs) => {
    loginUser(form.userName, form.password); //
  };

  return (
    <div className="login-container">
      <div className="flex justify-content-center align-items-center">
        <Card className="md:w-25rem">
          <form onSubmit={handleSubmit(handleLogin)}>
            <div className="flex flex-wrap justify-content-center align-items-center gap-2 mb-3">
              <label className="w-6rem">Username</label>
              <InputText {...register("userName")} />{" "}
              {errors.userName ? (
                <p className="text-red">{errors.userName.message}</p>
              ) : (
                ""
              )}
            </div>
            <div className="flex flex-wrap justify-content-center align-items-center gap-2 mb-3">
              <label className="w-6rem">Password</label>
              <InputText {...register("password")} />{" "}
              {errors.password ? (
                <p className="text-red">{errors.password.message}</p>
              ) : (
                ""
              )}
            </div>
            <Button
              label="Sign up
            "
            />
          </form>
        </Card>
      </div>
    </div>
  );
};

export default LoginPage;
