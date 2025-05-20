import { ACCESS_TOKEN } from "../Contains/Config";
import { callApi, callApiMuti } from "../Utils/clientApi";

export const getToken = () => {
  return localStorage.getItem(ACCESS_TOKEN);
};

export const apiClient = {
  fetchApiLogin(data) {
    return callApi("Users/login", "post", data);
  },

  fetchApiSignUp(data) {
    return callApi("Users/register", "post", data);
  },

  fetchApiUser() {
    return callApi("Users");
  },
  fetchApiUserAccountVerification() {
    return callApi("Users/AccountVerification");
  },
  fetchApiUserAccountVerificationPost(data) {
    return callApi("Users/AccountVerification", "post", data);
  },
  fetchApiUpdateImage(data) {
    return callApiMuti("Users/UploadImage", "put", data);
  },

  fetchApiChangePassword(data) {
    return callApi("Users/ChangePassword", "post", data);
  },

  fetchApiForgotPassWordCode(data) {
    return callApi("Users/ForgotPasswordCode", "post", data);
  },
  fetchApiForgotPassWord(data) {
    return callApi("Users/ForgotPassword", "post", data);
  },

  fetchApiGetUserID(idUser) {
    return callApi(`Users/GetById/${idUser}`);
  },

  // update info user
  fetchApiUpdateUser(data) {
    return callApi("Users", "put", data);
  },
  //Find User
  fetchApiFindUser(phoneNumber) {
    return callApi(`Users/FindUser/${phoneNumber}`, "get");
  },
  fetchApiPostUserProject(id, data) {
    return callApi(`MemberProject/${id}`, "post", data);
  },
  // Project

  fetchApiGetProjects() {
    return callApi("Projects");
  },

  fetApiCreateProject(data) {
    return callApi("Projects", "post", data);
  },

  fetchApiUpdateProject(id) {
    return callApi(`Projects/${id}`, "put");
  },

  fetchApiDeleteProject(id) {
    return callApi(`Projects/${id}`, "delete");
  },
  fetchApiSearchProject(name) {
    return callApi(`Projects/GetName/${name}`);
  },
  fetApiProject(id) {
    return callApi(`Projects/${id}`);
  },

  fetchApiGetNameBoard(name) {
    return callApi(`Projects/GetName/${name}`);
  },

  fetchApiGetProjectPerson() {
    return callApi("Projects/UserProject");
  },

  // Tabs

  fetApiCreateTab(id, data) {
    return callApi(`Tabs/${id}`, "post", data);
  },

  fetchApiUpdateTab(id, data) {
    return callApi(`Tabs/${id}`, "put", data);
  },

  fetchApiDeleteTab(id) {
    return callApi(`Tabs/${id}`, "delete");
  },

  fetchApiGetTabs(id) {
    return callApi(`Tabs/${id}`);
  },

  fetchApiGetIdTab(idTab) {
    return callApi(`Tabs/Id/${idTab}`);
  },

  fetchApiUpdateNameTab(idCol, data) {
    return callApi(`Tabs/Name/${idCol}`, "put", data);
  },

  // Tasks

  fetApiCreateTask(id, data) {
    return callApi(`Tasks/${id}`, "post", data);
  },

  fetchApiUpdateTask(id, data) {
    return callApi(`Tasks/${id}`, "put", data);
  },

  fetchApiDeleteTask(id) {
    return callApi(`Tasks/${id}`, "delete");
  },

  fetchApiGetTasks(id) {
    return callApi(`Tasks/${id}`);
  },

  fetchApiGetIdTask(idTask) {
    return callApi(`Tasks/Id/${idTask}`);
  },

  fetchApiChangeTask(idTask) {
    return callApi(`Tasks/Change/${idTask}`);
  },

  // Cards

  // Tabs

  fetchApiCreateCard(id, data) {
    return callApi(`Cards/${id}`, "post", data);
  },

  fetchApiUpdateCard(id, data) {
    return callApi(`Cards/${id}`, "put", data);
  },

  fetchApiDeleteCard(id) {
    return callApi(`Cards/${id}`, "delete");
  },

  fetchApiGetCards(id) {
    return callApi(`Cards/${id}`);
  },

  fetchApiGetIdCard(idCard) {
    return callApi(`Cards/Id/${idCard}`);
  },

  fetchApiUpdateCardName(id, data) {
    return callApi(`Cards/Name/${id}`, "put", data);
  },

  // PUT Move
  fetchApiMoveCard(idCard, idTabNew, idTabOld, number) {
    return callApi(
      `Cards/Move/${idCard}/${idTabNew}/${idTabOld}/${number}`,
      "put"
    );
  },
  fetchApiMoveNumberCard(idCard, number) {
    return callApi(`Cards/MoveOrder/${idCard}/${number}`, "put");
  },

  // CardUserMenber

  fetApiCreateCardUserMenber(cardId, userId) {
    return callApi(`CardUserMember/${cardId}/${userId}`, "post");
  },

  fetchApiDeleteCardUserMenber(cardId, userId) {
    return callApi(`CardUserMember/${cardId}/${userId}`, "delete");
  },

  fetchApiGetCardUserMenbers(cardId) {
    return callApi(`CardUserMember/${cardId}`);
  },

  // MenberProject

  fetApiCreateMemberProject(id, data) {
    return callApi(`MemberProject/${id}`, "post", data);
  },

  fetchApiDeleteMemberProject(idProject, idUser) {
    return callApi(`MemberProject/${idProject}/${idUser}`, "delete");
  },

  fetchApiGetMemberProjects(id) {
    return callApi(`MemberProject/${id}`);
  },

  // TaskMember

  fetApiCreateTaskUserMenber(TaskId, userId) {
    return callApi(`TaskUserMember/${TaskId}/${userId}`, "post");
  },

  fetchApiDeleteTaskUserMenber(TaskId, userId) {
    return callApi(`TaskUserMember/${TaskId}/${userId}`, "delete");
  },

  fetchApiGetTaskUserMenbers(TaskId) {
    return callApi(`TaskUserMember/${TaskId}`);
  },

  // Remind card members
  fetchApiRemindCardMembers(cardId) {
    return callApi(`Cards/Remind/${cardId}`, "post");
  },
};
