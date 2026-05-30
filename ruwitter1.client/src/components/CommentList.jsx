import React from "react";
import CommentItem from "./CommentItem";
import cl from "../pages/Posts/Posts.module.css";

const CommentList = ({ comments, isLikeLoading, handleLike }) => {

    return (
        <div className={cl.post_container}>
            <div className="comments">
                {comments.map((comment, index) =>
                    <CommentItem number={index + 1} comment={comment} key={comment.id} isLikeLoading={isLikeLoading} handleLike={handleLike} />
                )}
            </div>
        </div>
    );
};
export default CommentList;