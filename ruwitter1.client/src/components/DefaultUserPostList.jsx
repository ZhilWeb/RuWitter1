import React from "react";
import PostItem from "./PostItem";
import cl from "../pages/DefaultUserProfile/DefaultUserProfile.module.css";
import DefaultUserPostItem from "./DefaultUserPostItem";


const DefaultUserPostList = ({ posts, lastElement, isLikeLoading, handleLike, deletePost, anotherUserId }) => {

    if (!posts.length) {
        return (
            <div className={cl.post_container}>
                <div className={ cl.posts_title}>У пользователя нет добавленных записей</div>
            </div>
            
        );
    }

    return (
        <div className={cl.post_container}>
            <div className={ cl.posts_title}>Записи</div>
            <div className="posts">
                {posts.map((post, index) =>
                    <DefaultUserPostItem number={index + 1} post={post} key={post.id} isLikeLoading={isLikeLoading} handleLike={handleLike} deletePost={deletePost} anotherUserId={anotherUserId} />
                )}
            </div>
        </div>
    );
};

export default DefaultUserPostList;