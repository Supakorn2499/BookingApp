import React, { useContext, useState } from "react";
import { LanguageContext } from "../LanguageContext";
import translations from "../resources/LangApp";
import { useNavigate } from "react-router-dom";
import { AuthContext } from "../AuthContext";
import Swal from "sweetalert2";
import api from "./apiConfig";

export default function Login() {
  const { language, toggleLanguage } = useContext(LanguageContext);
  const t = translations[language];
  const { login } = useContext(AuthContext);
  const [username, setUsername] = useState("");
  const [password, setPassword] = useState("");
  const navigate = useNavigate();

  const handleLogin = async (e) => {
    e.preventDefault();

    try {
      const response = await api.post(
        "/Auth/Login",
        { username, password },
        { headers: { "Content-Type": "application/json" } }
      );
      const { token, role } = response.data;
      login(token);
      navigate("/");
    } catch (error) {
      if (error.response && error.response.status === 401) {
        Swal.fire({
          //icon: "error",
          title: t.signin_failed_title,
          text: t.signin_failed_msg,
          confirmButtonText: t.button_close,
        });
      } else {
        Swal.fire({
          //icon: "error",
          title: t.signin_failed_title,
          text: error.message || "Something went wrong. Please try again.",
        });
      }
    } finally {
    }
  };

  return (
    <div className="flex h-screen flex-1">
      <div className="flex flex-1 flex-col justify-center px-4 py-12 sm:px-6 lg:flex-none lg:px-20 xl:px-24">
        <div className="mx-auto w-full max-w-sm lg:w-96">
          <div className="text-center">
            <h2 className="mt-4 text-2xl/9 font-bold tracking-tight text-gray-900">
              {t.signin_title}
            </h2>
            <p>{t.signin_subtitle}</p>
          </div>
          <div className="mt-4">
            <div>
              <form onSubmit={handleLogin} className="space-y-6">
                <div>
                  <label
                    htmlFor="email"
                    className="block text-sm/6 font-medium text-gray-900"
                  >
                    {t.signin_username}
                  </label>
                  <div className="mt-1">
                    <input
                      id="username"
                      name="username"
                      type="text"
                      value={username}
                      onChange={(e) => setUsername(e.target.value)}
                      required
                      className="block w-full rounded-md border-0 py-1.5 shadow-sm ring-1 ring-inset ring-gray-300 placeholder:text-gray-400 focus:ring-2 focus:ring-inset focus:ring-indigo-600 sm:text-sm/6"
                    />
                  </div>
                </div>
                <div>
                  <label
                    htmlFor="password"
                    className="block text-sm/6 font-medium text-gray-900"
                  >
                    {t.signin_password}
                  </label>
                  <div className="mt-1">
                    <input
                      id="password"
                      name="password"
                      type="password"
                      value={password}
                      onChange={(e) => setPassword(e.target.value)}
                      required
                      autoComplete="current-password"
                      className="block w-full rounded-md border-0 py-1.5 shadow-sm ring-1 ring-inset ring-gray-300 placeholder:text-gray-400 focus:ring-2 focus:ring-inset focus:ring-indigo-600 sm:text-sm/6"
                    />
                  </div>
                </div>

                <div className="pt-0">
                  <button
                    type="submit"
                    className="flex w-full justify-center rounded-md bg-indigo-600 px-3 py-1.5 text-sm/6 font-semibold text-white shadow-sm hover:bg-indigo-500 focus-visible:outline focus-visible:outline-2 focus-visible:outline-offset-2 focus-visible:outline-indigo-600"
                  >
                    {t.signin_button_login}
                  </button>
                </div>
              </form>
            </div>
          </div>
          <div className="flex items-center justify-between">
            <div className="flex item-center text-sm">
              <span>Copy All. 2024</span>
            </div>
            <div className="text-right mt-2 font-semibold text-sm item-center">
              <span>{t.language_text}</span>
              <button
                onClick={toggleLanguage}
                className="px-4 py-2 ml-2 text-gray-600  hover:bg-gray-300 rounded-lg"
              >
                {language === "en" ? "ไทย" : "English"}
              </button>
            </div>
          </div>
        </div>
      </div>
      <div className="relative hidden w-0 flex-1 lg:block">
        <img
          alt=""
          src="https://images.unsplash.com/photo-1496917756835-20cb06e75b4e?ixlib=rb-1.2.1&ixid=eyJhcHBfaWQiOjEyMDd9&auto=format&fit=crop&w=1908&q=80"
          className="absolute inset-0 size-full object-cover"
        />
      </div>
    </div>
  );
}
