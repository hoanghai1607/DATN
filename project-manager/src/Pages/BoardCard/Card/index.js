import { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import GroundAvatar from "../../../Components/common/GroundAvatar";
import { apiClient } from "../../../Services";
import ModalCard from "../ModalCard";

const Card = (props) => {
  const { card } = props;
  const { boardId } = useParams();
  const [isModalVisible, setIsModalVisible] = useState(false);
  const [titleCard, setTitleCard] = useState("");
  const [descriptionCard, setDescriptionCard] = useState("");
  const [memberCard, setMemberCard] = useState([]);
  const [memberMoreOne, setMemberMoreOne] = useState([]);

  useEffect(() => {
    apiClient.fetchApiGetMemberProjects(boardId).then((res) => {
      if (res.data) {
        setMemberCard(res.data);
      } else {
      }
    });
  }, []);

  useEffect(() => {
    apiClient.fetchApiGetCardUserMenbers(card.id).then((res) => {
      if (res.data) {
        setMemberMoreOne(res.data);
      } else {
        // console.log("Fail.....")
      }
    });
  }, []);

  useEffect(() => {
    if (card && card.name) {
      setTitleCard(card.name);
    }
    if (card && card.description) {
      setDescriptionCard(card.description);
    }
  }, []);

  const showModal = () => {
    setIsModalVisible(true);
  };

  const handleOk = (name, description) => {
    card.name = name;
    card.description = description;
    setIsModalVisible(false);
  };

  const handleCancel = () => {
    setIsModalVisible(false);
  };

  // Manager Member Card
  const handleUpdateCardMember = (user) => {
    const userExist = memberMoreOne.filter((i, _i) => user.id === i.id);
    if (userExist && userExist.length > 0) {
      apiClient.fetchApiDeleteCardUserMenber(card.id, user.id).then((res) => {
        // console.log(res.data);
      });
      let index = memberMoreOne.findIndex((i) => i.id === user.id);
      memberMoreOne.splice(index, 1);
      setMemberMoreOne([...memberMoreOne]);
    } else {
      apiClient.fetApiCreateCardUserMenber(card.id, user.id);
      setMemberMoreOne([...memberMoreOne, user]);
    }
  };

  return (
    <>
      <li className="card-item" onClick={showModal}>
        {card.image && (
          <img
            className="card-cover"
            src={""}
            onMouseDown={(e) => e.preventDefault()}
          />
        )}
        {card.name}
        <div className="user-card">
          <GroundAvatar member={memberMoreOne} />
        </div>
      </li>

      <ModalCard
        card={card}
        handleOk={handleOk}
        handleCancel={handleCancel}
        isModalVisible={isModalVisible}
        titleCard={titleCard}
        setTitleCard={setTitleCard}
        descriptionCard={descriptionCard}
        setDescriptionCard={setDescriptionCard}
        memberCard={memberCard}
        memberMoreOne={memberMoreOne}
        handleUpdateCardMember={handleUpdateCardMember}
      />
    </>
  );
};

export default Card;
