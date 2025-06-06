import React from "react";
import MyButton from "./UI/button/MyButton";
// import { useNavigate } from "react-router";


const PostItem = (props) => {
    // let router = useNavigate();



    return (
        <div className="post">
            <div className="post__content">
                <strong>{props.post.id}. {props.post.title}</strong>
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
            </div>
        </div>
    );
};

export default PostItem;