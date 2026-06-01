import React, { useEffect, useMemo, useRef, useState } from "react";
import { Layout } from 'antd';
import HeaderRuW from "../../components/HeaderRuW/HeaderRuW";
import ContentRuW from "../../components/ContentRuW/ContentRuW";
import SidebarRuW from "../../components/SidebarRuW/SidebarRuW";
import cl from "../Posts/Posts.module.css";
import clpost from "./CreatePost.module.css";
import PostService from "../../API/PostService";
import PostForm from "../../components/PostForm";

function CreatePost() {
    const { Header, Content, Footer, Sider } = Layout;


    const [isActiveSidebar, setActiveSidebar] = useState(false);

    const ToggleSidebar = () => {
        setActiveSidebar(!isActiveSidebar);
    };

    const createPost = async (newPost) => {
        await PostService.createPost(newPost.body, newPost.files);
        window.location.reload();
    };

    return (
        <Layout className={cl.App}>
            <HeaderRuW activeSidebar={ToggleSidebar} />
            <Layout className={clpost.post_main_container}>
                <SidebarRuW isActive={isActiveSidebar} />
                <ContentRuW>
                    <PostForm create={createPost} />
                </ContentRuW>
            </Layout>

        </Layout>
    );
}

export default CreatePost;