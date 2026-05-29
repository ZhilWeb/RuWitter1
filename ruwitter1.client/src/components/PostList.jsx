import React from "react";
import PostItem from "./PostItem";
import cl from "../pages/Posts/Posts.module.css";

const PostList = ({ posts, lastElement }) => {

    if (!posts.length) {
        return (
            <h1 style={{ textAlign: 'center' }}>
                Постов ещё нет... Добавьте первый!
            </h1>
        );
    }

    return (
        <div className={cl.posts }>
            {posts.map((post, index) =>
                <PostItem number={index + 1} post={post} key={post.id}/>
            )}
            <div ref={lastElement} style={{ background: '#FF0000', height: 20 }} />
        </div>
    );
};

export default PostList;