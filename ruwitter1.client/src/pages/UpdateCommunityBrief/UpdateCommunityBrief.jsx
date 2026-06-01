import React, { useEffect, useMemo, useRef, useState } from "react";
import { Layout } from 'antd';
import HeaderRuW from "../../components/HeaderRuW/HeaderRuW";
import ContentRuW from "../../components/ContentRuW/ContentRuW";
import SidebarRuW from "../../components/SidebarRuW/SidebarRuW";
import cl from "../Posts/Posts.module.css";
import clpost from "../CreatePost/CreatePost.module.css";
import PostService from "../../API/PostService";
import PostForm from "../../components/PostForm";
import UpdateCommunityProfile from "../../components/UpdateCommunityProfile";

function UpdateCommunityBrief() {
    let params = new URLSearchParams(window.location.search);
    let communityId = params.get('id');
    console.log(communityId);

    const { Header, Content, Footer, Sider } = Layout;
    const [communitiesCategories, setCommunitiesCategories] = useState([]);

    const [isActiveSidebar, setActiveSidebar] = useState(false);

    const ToggleSidebar = () => {
        setActiveSidebar(!isActiveSidebar);
    };

    const updateProfile = async (newProfile, avatar) => {
        await PostService.updateCommunityProfile(newProfile, avatar, communityId);
        alert("Сообщество изменено.");
        window.location.href = `/community?id=${communityId}`;
    };

    useEffect(() => {

        const loadCategories = async () => {
            try {
                let categories = await PostService.getCommunititesCategories();
                console.log(categories);
                setCommunitiesCategories(categories);
            } catch (e) {
                console.error("Не удалось загрузить категории сообществ", e);
            }
        };
        loadCategories();
    }, []);

    return (
        <Layout className={cl.App}>
            <HeaderRuW activeSidebar={ToggleSidebar} />
            <Layout className={clpost.post_main_container}>
                <SidebarRuW isActive={isActiveSidebar} />
                <ContentRuW>
                    <UpdateCommunityProfile update={updateProfile} communitiesCategories={communitiesCategories} communityId={communityId} buttonText={'Изменить'} />
                </ContentRuW>
            </Layout>

        </Layout>
    );
}
export default UpdateCommunityBrief;