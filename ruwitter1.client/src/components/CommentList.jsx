import React from "react";
import CommentItem from "./CommentItem";
import cl from "../pages/Posts/Posts.module.css";
import clid from "../pages/PostIdPage/PostIdPage.module.css";

const CommentList = ({ comments, isLikeLoading, handleLike }) => {

    return (
        <div className={clid.comment_container}>
                {comments.map((comment, index) =>
                    <CommentItem number={index + 1} comment={comment} key={comment.id} isLikeLoading={isLikeLoading} handleLike={handleLike} />
                )}

        </div>
    );
};
export default CommentList;