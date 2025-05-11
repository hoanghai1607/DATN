import React from "react";
import { Link } from "react-router-dom";
import { BACKGROUD_BOARD } from "../../actions/dataBackgroud";
import "./BoardCard.scss";
function BoardCard(props) {

  const {boardPerson} = props;
  // const fakeData = [
  //   {
  //     img: 'https://avi.edu.vn/wp-content/uploads/2019/11/london-2393098.jpg',
  //     title: 'Đồ án Web'
  //   },
  //   {
  //     img: 'https://deviet.vn/wp-content/uploads/2019/04/vuong-quoc-anh.jpg',
  //     title: 'Đồ án di động'
  //   },

  // ]
  return (
    <div className="card-container">
      {boardPerson.map((data, key) => (
        <Link key={key} to={`/board/${data.id}/schedule`}>
          <div className="content">
            <img
              className="content-img"
              alt=""
              src={BACKGROUD_BOARD[data.background]}
            />
            <p className="content-title">
              {data.name}
            </p>
          </div>
        </Link>
      ))}
    </div>
  );
}

export default BoardCard;
