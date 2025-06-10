import React from "react";
import MyButton from "./UI/button/MyButton";
import { CommentOutlined } from '@ant-design/icons';
// import { useNavigate } from "react-router";
// import { NavLink } from "react-router";
import { Link } from 'react-router-dom';

const PostItem = (props) => {
    // let router = useNavigate();



    return (
        <div className="post">
            <div className="post__content">
                <img src={`data:${props.post.user.avatar.contentType};base64,${props.post.user.avatar.data}`} height="400px" />
                <strong>{props.post.user.nickname}. {props.post.publicDate}</strong>
                <br></br>
                <p>{props.post.body}</p>
                {props.post.mediaFiles.map((mediaFile) =>
                    <img src={`data:${mediaFile.contentType};base64,${mediaFile.data}`} height="400px" key={mediaFile.id} />
                )}
                
            </div>
            <div className="post__btns">
                {/*onClick={() => router(`/posts/${props.post.id}`)}*/ }
                <MyButton>Открыть</MyButton>
                <MyButton onClick={() => props.remove(props.post)}>Удалить</MyButton>
                <Link to={`/post?id=${props.post.id}`}><CommentOutlined style={{ fontSize: '40px' }} /></Link>
            </div>
        </div>
    );
};

export default PostItem;