import React, { useEffect, useState } from "react";
import { Layout, Avatar, Card, Row, Col, Input, InputNumber, Button, Space, Select } from "antd";
import { UserOutlined, TeamOutlined, SearchOutlined, EnvironmentOutlined, PhoneOutlined } from '@ant-design/icons';
import HeaderRuW from "../../components/HeaderRuW/HeaderRuW";
import ContentRuW from "../../components/ContentRuW/ContentRuW";
import SidebarRuW from "../../components/SidebarRuW/SidebarRuW";
import PostService from "../../API/PostService";
import { useFetching } from "../../hooks/useFetching";
import cl from "../SearchPage/SearchPage.module.css";
import clposts from "../Posts/Posts.module.css";
import { Link } from 'react-router-dom';


function MyCommunities() {

    const [communitiesResults, setCommunitiesResults] = useState([]);
    const [communitiesCategories, setCommunitiesCategories] = useState([]);
    const [isActiveSidebar, setActiveSidebar] = useState(false);

    const [fetchCommunities, isCommunitiesLoading, communitiesError] = useFetching(async () => {

        const data = await PostService.getCurrentUserCommunities();
        console.log(data);
        // Дополнительное обогащение аватарками, если в результатах приходят только avatarId
        const enrichedUsers = [...data];
        for (let user of enrichedUsers) {
            if (user.avatarId && !user.avatar) {
                user.avatar = await PostService.getAvatarById(user.avatarId);
            }
        }
        setCommunitiesResults(enrichedUsers);
    });


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
        fetchCommunities();
        loadCategories();
    }, []);

    const ToggleSidebar = () => {
        setActiveSidebar(!isActiveSidebar);
    };

    return (
        <Layout style={{ minHeight: "100vh" }}>
            <HeaderRuW activeSidebar={ToggleSidebar} />
            <Layout className={clposts.main_container}>
                <SidebarRuW isActive={isActiveSidebar} />
                <ContentRuW>
                    <div className={cl.searchWrapper}>

                        
                        <div className={cl.resultsContainer}>
                            <h3 className={cl.sectionTitle}>Мои сообщества:</h3>
                            <Link to={'/createcommunity'} className={cl.submitBtn}>
                                Создать сообщество
                            </Link>
                            {communitiesError && <p className={cl.error}>Ошибка: {communitiesError}</p>}
                            {communitiesResults.map(comm => (
                                <Link to={`/community?id=${comm.id}`}>
                                    <Card key={comm.id} className={cl.resultCard} hoverable>
                                        <Card.Meta
                                            avatar={
                                                comm.avatar?.data ? (
                                                    <img src={`data:${comm.avatar.contentType};base64,${comm.avatar.data}`} className={cl.commAvatar} alt="" />
                                                ) : (
                                                    <Avatar size={54} icon={<TeamOutlined />} />
                                                )
                                            }
                                            title={<span className={cl.itemTitle}>{comm.name}</span>}
                                            description={
                                                <div className={cl.itemMeta}>
                                                    <p><strong>Категория:</strong> {
                                                        communitiesCategories.find(cat => cat.id === comm.communityCategoryId)?.name || "Не указана"
                                                    }</p>
                                                    {comm.briefInformation && <p className={cl.briefBlock}>{comm.briefInformation}</p>}
                                                </div>
                                            }
                                        />
                                    </Card>
                                </Link>
                            ))}
                            {!isCommunitiesLoading && communitiesResults.length === 0 && <p className={cl.empty}>Список пуст. Добавьте новое сообщество.</p>}
                        </div>

                    </div>
                </ContentRuW>
            </Layout>
        </Layout>
    );
}

export default MyCommunities;