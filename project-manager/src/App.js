import { useEffect } from "react";
import { BrowserRouter as Router, Route, Routes } from "react-router-dom";
import HomePage from "./Pages/HomePage";
import "antd/dist/antd.css";
import WorkSpaces from "./Pages/WorkSpaces";
import { library } from "@fortawesome/fontawesome-svg-core";
import { fab } from "@fortawesome/free-brands-svg-icons";
import { fas } from "@fortawesome/free-solid-svg-icons";
import "./App.css";
import LoginPage from "./Pages/LoginPage";
import RegisterPage from "./Pages/RegisterPage";
import BroadContext from "./Pages/BoardCard/BroadContext";
import { ACCESS_TOKEN } from "./Contains/Config";
import Settings from "./Pages/Settings";
import "react-toastify/dist/ReactToastify.css";
import { ToastContainer } from "react-toastify";
import ForgotPassWordPage from "./Pages/ForgotPassWordPage";
import ResumeUser from "./Pages/ResumeUser";
import ProjectManagement from "./Pages/ProjectManagement";

library.add(fab, fas);
function App() {
  const token = localStorage.getItem(ACCESS_TOKEN);
  return (
    <>
      <ToastContainer />
      <Router>
        <Routes>
          <Route path="/" element={<HomePage />} />
          <Route path="/board/:boardId/schedule" element={<BroadContext />} />
          <Route path="/settings" element={<Settings />} />
          <Route path="/workspace" element={<WorkSpaces />} />
          <Route path="/forgot-password" element={<ForgotPassWordPage />} />
          <Route path="/resume-user/:userID" element={<ResumeUser />} />
          <Route path="/project-management" element={<ProjectManagement />} />

          <Route path="/login" element={<LoginPage />} />
          <Route path="/sign-up" element={<RegisterPage />} />
        </Routes>
      </Router>
    </>
  );
}

export default App;
