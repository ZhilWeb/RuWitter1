import React from "react";
import PostItem from "./PostItem";
import cl from "../pages/Posts/Posts.module.css";

const PostList = ({ posts, lastElement, isLikeLoading, handleLike }) => {

    if (!posts.length) {
        return (
            <h1 style={{ textAlign: 'center' }}>
                Постов ещё нет... Добавьте первый!
            </h1>
        );
    }

    return (
        <div className={cl.post_container}>
            <div className="posts">
                {posts.map((post, index) =>
                    <PostItem number={index + 1} post={post} key={post.id} isLikeLoading={isLikeLoading} handleLike={handleLike} />
                    )}
            </div>
            <div ref={lastElement} style={{ background: '#FF0000', height: 20 }} />
        </div>
    );
};

export default PostList;