import React from "react";
import * as Yup from "yup";
import { yupResolver } from "@hookform/resolvers/yup";
import { useForm } from "react-hook-form";
import { useAuth } from "../../../Context/useAuth.tsx";
import { InputText } from "primereact/inputtext";
import { Button } from "primereact/button";
import { useNavigate } from "react-router-dom";

type Props = {};

type RegisterFormInputs = {
  email: string;
  userName: string;
  password: string;
  confirmPassword: string;
};

const validationSchema = Yup.object().shape({
  email: Yup.string()
    .email("Invalid email address")
    .required("Email is required"),
  userName: Yup.string().required("Username is required"),
  password: Yup.string()
    .min(6, "Password must be at least 6 characters long")
    .matches(
      /[a-z]/,
      "Passwords must have at least one lowercase letter ('a'-'z')."
    )
    .matches(
      /[A-Z]/,
      "Passwords must have at least one uppercase letter ('A'-'Z')."
    )
    .required("Password is required"),
  confirmPassword: Yup.string()
    .oneOf([Yup.ref("password"), undefined], "Passwords must match")
    .required("Confirm password is required"),
});

const RegisterPage = (props: Props) => {
  const { registerUser } = useAuth();
  const {
    register,
    handleSubmit,
    formState: { errors },
  } = useForm<RegisterFormInputs>({
    resolver: yupResolver(validationSchema),
  });

  const [errorMessage, setErrorMessage] = React.useState<string | null>(null);

  const navigate = useNavigate();

  const handleRegister = async (form: RegisterFormInputs) => {
    try {
      await registerUser(form.email, form.userName, form.password);
    } catch (error: any) {
      if (error.message) {
        setErrorMessage(error.message);
      }
    }
  };

  return (
    <div
      className="flex justify-content-center"
      style={{
        marginTop: "20vh",
      }}
    >
      <form onSubmit={handleSubmit(handleRegister)}>
        <div
          className="flex flex-column px-8 py-5 gap-4"
          style={{
            maxWidth: "500px",
            borderRadius: "12px",
            background: "#1e1e1e",
          }}
        >
          <div className="inline-flex flex-column gap-2">
            <label className="text-primary-50 font-semibold">Email</label>
            <InputText
              {...register("email")}
              className="bg-white-alpha-20 border-none p-3 text-primary-50"
            />
            {errors.email && <p className="text-red">{errors.email.message}</p>}
          </div>

          <div className="inline-flex flex-column gap-2">
            <label className="text-primary-50 font-semibold">Username</label>
            <InputText
              {...register("userName")}
              className="bg-white-alpha-20 border-none p-3 text-primary-50"
            />
            {errors.userName && (
              <p className="text-red">{errors.userName.message}</p>
            )}
          </div>

          <div className="inline-flex flex-column gap-2">
            <label className="text-primary-50 font-semibold">Password</label>
            <InputText
              {...register("password")}
              type="password"
              className="bg-white-alpha-20 border-none p-3 text-primary-50"
            />
            {errors.password && (
              <p className="text-red">{errors.password.message}</p>
            )}
          </div>

          <div className="inline-flex flex-column gap-2">
            <label className="text-primary-50 font-semibold">
              Confirm Password
            </label>
            <InputText
              {...register("confirmPassword")}
              type="password"
              className="bg-white-alpha-20 border-none p-3 text-primary-50"
            />
            {errors.confirmPassword && (
              <p className="text-red">{errors.confirmPassword.message}</p>
            )}
          </div>

          <div>
            {errorMessage && <p className="text-red mt-2">{errorMessage}</p>}
          </div>

          <div className="inline-flex flex-column gap-2">
            <Button
              type="submit"
              label="Register"
              className="p-3 w-full text-primary-50 border-1 border-primary hover:bg-primary-700"
            />
          </div>
        </div>
      </form>
    </div>
  );
};

export default RegisterPage;
