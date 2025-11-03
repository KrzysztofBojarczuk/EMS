import React from "react";
import { useForm, Controller } from "react-hook-form";
import { InputText } from "primereact/inputtext";
import { Button } from "primereact/button";
import { useNavigate } from "react-router-dom";
import { useAuth } from "../../../Context/useAuth";

const LoginPage = () => {
  const { loginUser } = useAuth();
  const navigate = useNavigate();
  const [errorMessage, setErrorMessage] = React.useState<string | null>(null);

  const {
    control,
    handleSubmit,
    formState: { errors },
  } = useForm({
    defaultValues: {
      userName: "",
      password: "",
    },
  });

  const handleLogin = async (data: { userName: string; password: string }) => {
    try {
      await loginUser(data.userName, data.password);
    } catch (error: any) {
      if (error.message) {
        setErrorMessage(error.message);
      }
    }
  };

  return (
    <div className="flex justify-content-center" style={{ marginTop: "30vh" }}>
      <form onSubmit={handleSubmit(handleLogin)}>
        <div
          className="flex flex-column px-8 py-5 gap-4"
          style={{
            maxWidth: "500px",
            borderRadius: "12px",
            background: "#1e1e1e",
          }}
        >
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
          {errorMessage && <p className="text-red mt-2">{errorMessage}</p>}
          <div className="inline-flex flex-column gap-2">
            <Button type="submit" label="Sign In" />
            <Button
              type="button"
              label="Create Account"
              onClick={() => navigate("/register")}
            />
          </div>
        </div>
      </form>
    </div>
  );
};

export default LoginPage;
