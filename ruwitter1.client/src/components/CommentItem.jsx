import React from "react";
import MyButton from "./UI/button/MyButton";
import { CommentOutlined, HeartFilled, HeartOutlined } from '@ant-design/icons';
import { Link } from 'react-router-dom';
import cl from "../pages/Posts/Posts.module.css";
import clid from "../pages/PostIdPage/PostIdPage.module.css";
import { format } from 'date-fns';
import { ru } from 'date-fns/locale';

const CommentItem = (props) => {
    // let router = useNavigate();

    const date = new Date(props.comment.publicDate);
    console.log(props.comment.hasLike);

    return (
        <div className={clid.comment}>
            <div className={cl.post_content}>
                <div className={cl.avatar_name}>
                    {props.comment.user.avatar && <img src={`data:${props.comment.user.avatar.contentType};base64,${props.comment.user.avatar.data}`} height="400px" className={cl.avatar} />}
                    <div className={cl.username}>{props.comment.user.nickname}</div>
                </div>

                <br></br>
                <p className={cl.post_body}>{props.comment.body}</p>
                {props.comment.mediaFiles && props.comment.mediaFiles.map((mediaFile) =>
                    <img src={`data:${mediaFile.contentType};base64,${mediaFile.data}`} height="400px" key={mediaFile.id} />
                )}
                <div className={cl.post_btns}>
                    {/*onClick={() => router(`/posts/${props.post.id}`)}*/}
                    {props.comment.hasLike ?
                        <HeartFilled style={{ fontSize: '40px', color: 'red' }} disabled={props.isLikeLoading} onClick={() => props.handleLike(props.comment)} /> :
                        <HeartOutlined style={{ fontSize: '40px', color: 'red' }} disabled={props.isLikeLoading} onClick={() => props.handleLike(props.comment)} />}
                    {/*<MyButton onClick={() => props.remove(props.post)}>Удалить</MyButton>*/}

                </div>
                {/*<div className={cl.post_data}>{Intl.DateTimeFormat('ru-RU', {
                    hour: '2-digit',
                    minute: '2-digit',
                    day: 'numeric',
                    month: 'numeric',
                    year: 'numeric',
                }).format(date)}</div>*/}
                {<div className={cl.post_data}>{format(date, "HH:mm d MMMM yyyy", { locale: ru })}</div>}
                
            </div>
            <hr></hr>
        </div>
    );
};

export default CommentItem;