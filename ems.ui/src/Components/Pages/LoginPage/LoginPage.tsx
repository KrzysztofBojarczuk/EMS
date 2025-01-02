import React from "react";
import * as Yup from "yup";
import { yupResolver } from "@hookform/resolvers/yup";
import { useForm } from "react-hook-form";
import { useAuth } from "../../../Context/useAuth.tsx";
import { InputText } from "primereact/inputtext";
import { Button } from "primereact/button";
import { useNavigate } from "react-router-dom";

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

  const handleLogin = async (form: LoginFormsInputs) => {
    try {
      await loginUser(form.userName, form.password);
    } catch (error: any) {
      if (error.message) {
        setErrorMessage(error.message);
      }
    }
  };

  const navigate = useNavigate();

  const [errorMessage, setErrorMessage] = React.useState<string | null>(null);

  return (
    <div
      className="flex justify-content-center"
      style={{
        marginTop: "30vh",
      }}
    >
      <form onSubmit={handleSubmit(handleLogin)}>
        <div
          className="flex flex-column px-8 py-5 gap-4"
          style={{
            maxWidth: "500px",
            borderRadius: "12px",
            background: "#1e1e1e",
          }}
        >
          <div className="inline-flex flex-column gap-2">
            <label className="text-primary-50 font-semibold">Username</label>
            <InputText
              {...register("userName", { required: "Username is required" })}
              className="bg-white-alpha-20 border-none p-3 text-primary-50"
            />
            {errors.userName && (
              <p className="text-red">{errors.userName.message}</p>
            )}
          </div>

          <div className="inline-flex flex-column gap-2">
            <label className="text-primary-50 font-semibold">Password</label>
            <InputText
              {...register("password", { required: "Password is required" })}
              type="password"
              className="bg-white-alpha-20 border-none p-3 text-primary-50"
            />
            {errors.password && (
              <p className="text-red">{errors.password.message}</p>
            )}
          </div>

          <div>
            {errorMessage && <p className="text-red mt-2">{errorMessage}</p>}
          </div>

          <div className="inline-flex flex-column gap-2 ">
            <Button
              type="submit"
              label="Sign In"
              className="p-3 w-full text-primary-50 border-1 border-primary hover:bg-primary-700"
            />
            <Button
              type="submit"
              label="Create Account"
              className="p-3 w-full text-primary-50 border-1 border-primary hover:bg-primary-700"
              onClick={() => navigate("/register")}
            />
          </div>
        </div>
      </form>
    </div>
  );
};

export default LoginPage;
