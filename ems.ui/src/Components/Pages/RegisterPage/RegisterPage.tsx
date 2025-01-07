import React from "react";
import { useForm, Controller } from "react-hook-form";
import { InputText } from "primereact/inputtext";
import { Button } from "primereact/button";
import { useAuth } from "../../../Context/useAuth.tsx";
import { useNavigate } from "react-router-dom";

const RegisterPage = () => {
  const { registerUser } = useAuth();
  const navigate = useNavigate();
  const [errorMessage, setErrorMessage] = React.useState<string | null>(null);

  const {
    control,
    handleSubmit,
    formState: { errors },
  } = useForm({
    defaultValues: {
      email: "",
      userName: "",
      password: "",
      confirmPassword: "",
    },
  });

  const handleRegister = async (data: {
    email: string;
    userName: string;
    password: string;
    confirmPassword: string;
  }) => {
    if (data.password !== data.confirmPassword) {
      setErrorMessage("Passwords do not match");
      return;
    }
    try {
      await registerUser(data.email, data.userName, data.password);
    } catch (error: any) {
      if (error.message) {
        setErrorMessage(error.message);
      }
    }
  };

  return (
    <div className="flex justify-content-center" style={{ marginTop: "20vh" }}>
      <form onSubmit={handleSubmit(handleRegister)}>
        <div
          className="flex flex-column px-8 py-5 gap-4"
          style={{
            maxWidth: "500px",
            borderRadius: "12px",
            background: "#1e1e1e",
          }}
        >
          <Controller
            name="email"
            control={control}
            rules={{
              required: "Email is required",
              pattern: {
                value: /^[^\s@]+@[^\s@]+\.[^\s@]+$/,
                message: "Invalid email format",
              },
            }}
            render={({ field }) => (
              <div className="inline-flex flex-column gap-2">
                <label className="text-primary-50 font-semibold">Email</label>
                <InputText {...field} />
                {errors.email && (
                  <small className="p-error">{errors.email.message}</small>
                )}
              </div>
            )}
          />
          <Controller
            name="userName"
            control={control}
            rules={{ required: "Username is required" }}
            render={({ field }) => (
              <div className="inline-flex flex-column gap-2">
                <label className="text-primary-50 font-semibold">
                  Username
                </label>
                <InputText {...field} />
                {errors.userName && (
                  <small className="p-error">{errors.userName.message}</small>
                )}
              </div>
            )}
          />
          <Controller
            name="password"
            control={control}
            rules={{ required: "Password is required" }}
            render={({ field }) => (
              <div className="inline-flex flex-column gap-2">
                <label className="text-primary-50 font-semibold">
                  Password
                </label>
                <InputText {...field} type="password" />
                {errors.password && (
                  <small className="p-error">{errors.password.message}</small>
                )}
              </div>
            )}
          />
          <Controller
            name="confirmPassword"
            control={control}
            rules={{ required: "Confirm password is required" }}
            render={({ field }) => (
              <div className="inline-flex flex-column gap-2">
                <label className="text-primary-50 font-semibold">
                  Confirm Password
                </label>
                <InputText {...field} type="password" />
                {errors.confirmPassword && (
                  <small className="p-error">
                    {errors.confirmPassword.message}
                  </small>
                )}
              </div>
            )}
          />
          {errorMessage && <p className="text-red mt-2">{errorMessage}</p>}
          <div className="inline-flex flex-column gap-2">
            <Button type="submit" label="Register" />
          </div>
        </div>
      </form>
    </div>
  );
};

export default RegisterPage;
