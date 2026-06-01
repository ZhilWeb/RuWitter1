// SearchPage.jsx
import React, { useEffect, useState } from "react";
import { Layout, Avatar, Card, Row, Col, Input, InputNumber, Button, Space, Select } from "antd";
import { UserOutlined, TeamOutlined, SearchOutlined, EnvironmentOutlined, PhoneOutlined } from '@ant-design/icons';
import HeaderRuW from "../../components/HeaderRuW/HeaderRuW";
import ContentRuW from "../../components/ContentRuW/ContentRuW";
import SidebarRuW from "../../components/SidebarRuW/SidebarRuW";
import PostService from "../../API/PostService";
import { useFetching } from "../../hooks/useFetching";
import cl from "./SearchPage.module.css";
import clposts from "../Posts/Posts.module.css";
import { Link } from 'react-router-dom';

function SearchPage() {
    const [searchMode, setSearchMode] = useState("users"); // Переключение между "users" и "communities"
    const [isActiveSidebar, setActiveSidebar] = useState(false);
    const [communitiesCategories, setCommunitiesCategories] = useState([]);

    // Результаты поиска
    const [usersResults, setUsersResults] = useState([]);
    const [communitiesResults, setCommunitiesResults] = useState([]);

    // 1. Поля формы компонента поиска ПОЛЬЗОВАТЕЛЕЙ
    const [userFilters, setUserFilters] = useState({
        phoneNumber: "",
        nickname: "",
        ageFrom: 1,
        ageTo: 100,
        city: "",
        interests: ""
    });

    // 2. Поля формы компонента поиска СООБЩЕСТВ
    const [communityFilters, setCommunityFilters] = useState({
        name: "",
        briefInformationSubstring: "",
        managerName: "",
        communityCategoryIds: []
    });

    // Стейт для парсинга ID категорий (вводятся пользователем через запятую, например "1, 3, 4")
    const [categoriesInput, setCategoriesInput] = useState("");

    // Хук для загрузки пользователей
    const [fetchUsers, isUsersLoading, usersError] = useFetching(async () => {
        const data = await PostService.searchUsers(userFilters);

        // Дополнительное обогащение аватарками, если в результатах приходят только avatarId
        const enrichedUsers = [...data];
        for (let user of enrichedUsers) {
            if (user.avatarId && !user.avatar) {
                user.avatar = await PostService.getAvatarById(user.avatarId);
            }
        }
        setUsersResults(enrichedUsers);
    });

    // Хук для загрузки сообществ
    const [fetchCommunities, isCommunitiesLoading, communitiesError] = useFetching(async () => {

        const data = await PostService.searchCommunities(communityFilters);
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
        loadCategories();
    }, []);

    

    const handleUsersSubmit = (e) => {
        e.preventDefault();
        fetchUsers();
    };

    const handleCommunitiesSubmit = (e) => {
        e.preventDefault();
        fetchCommunities();
    };

    const ToggleSidebar = () => setActiveSidebar(!isActiveSidebar);


    return (
        <Layout style={{ minHeight: "100vh" }}>
            <HeaderRuW activeSidebar={ToggleSidebar} />
            <Layout className={clposts.main_container}>
                <SidebarRuW isActive={isActiveSidebar} />
                <ContentRuW>
                    <div className={cl.searchWrapper}>

                        {/* Компонент переключения режимов с помощью тега <select> */}
                        <div className={cl.modePanel}>
                            <label htmlFor="mode-select" className={cl.modeLabel}>Раздел поиска: </label>
                            <select
                                id="mode-select"
                                value={searchMode}
                                onChange={(e) => setSearchMode(e.target.value)}
                                className={cl.nativeSelect}
                            >
                                <option value="users">Поиск Пользователей</option>
                                <option value="communities">Поиск Сообществ</option>
                            </select>
                        </div>

                        {/* ================= КОМПОНЕНТ ПОИСКА ПОЛЬЗОВАТЕЛЕЙ ================= */}
                        {searchMode === "users" && (
                            <div className={cl.searchSection}>
                                <form onSubmit={handleUsersSubmit} className={cl.searchForm}>
                                    <h3 className={cl.sectionTitle}>Настройки фильтрации людей</h3>
                                    <Row gutter={[16, 16]}>
                                        <Col xs={24} sm={12} md={8}>
                                            <Input
                                                placeholder="Никнейм"
                                                value={userFilters.nickname}
                                                onChange={e => setUserFilters({ ...userFilters, nickname: e.target.value })}
                                            />
                                        </Col>
                                        <Col xs={24} sm={12} md={8}>
                                            <Input
                                                placeholder="Номер телефона"
                                                prefix={<PhoneOutlined />}
                                                value={userFilters.phoneNumber}
                                                onChange={e => setUserFilters({ ...userFilters, phoneNumber: e.target.value })}
                                            />
                                        </Col>
                                        <Col xs={24} sm={12} md={8}>
                                            <Input
                                                placeholder="Город"
                                                prefix={<EnvironmentOutlined />}
                                                value={userFilters.city}
                                                onChange={e => setUserFilters({ ...userFilters, city: e.target.value })}
                                            />
                                        </Col>
                                        <Col xs={24} sm={12} md={8}>
                                            <Space>
                                                <InputNumber
                                                    placeholder="Возраст от"
                                                    value={userFilters.ageFrom}
                                                    min={1} max={100}
                                                    onChange={val => setUserFilters({ ...userFilters, ageFrom: val })}
                                                />
                                                <span>—</span>
                                                <InputNumber
                                                    placeholder="до"
                                                    value={userFilters.ageTo}
                                                    min={1} max={100}
                                                    onChange={val => setUserFilters({ ...userFilters, ageTo: val })}
                                                />
                                            </Space>
                                        </Col>
                                        <Col xs={24} sm={12} md={16}>
                                            <Input
                                                placeholder="Интересы"
                                                value={userFilters.interests}
                                                onChange={e => setUserFilters({ ...userFilters, interests: e.target.value })}
                                            />
                                        </Col>
                                    </Row>
                                    <Button type="primary" htmlType="submit" icon={<SearchOutlined />} loading={isUsersLoading} className={cl.submitBtn}>
                                        Выполнить поиск
                                    </Button>
                                </form>

                                {usersError && <p className={cl.error}>Ошибка: {usersError}</p>}

                                {/* Вывод списка пользователей */}
                                <div className={cl.resultsContainer}>
                                    {usersResults.map(user => (
                                        <Link to={`/user?id=${user.userId}` }>
                                            <Card key={user.userId} className={cl.resultCard} hoverable>
                                                <Card.Meta
                                                    avatar={
                                                        user.avatar?.data ? (
                                                            <img src={`data:${user.avatar.contentType};base64,${user.avatar.data}`} className={cl.searchAvatar} alt="" />
                                                        ) : (
                                                            <Avatar size={54} icon={<UserOutlined />} />
                                                        )
                                                    }
                                                    title={<span className={cl.itemTitle}>{user.nickname || "Пользователь"}</span>}
                                                    description={
                                                        <div className={cl.itemMeta}>
                                                            <p><strong>Возраст:</strong> {user.age} | <strong>Город:</strong> {user.city || "Не указан"}</p>
                                                            {user.phoneNumber && <p><strong>Телефон:</strong> {user.phoneNumber}</p>}
                                                            {user.interests && <p><strong>Интересы:</strong> {user.interests}</p>}
                                                            {user.briefInformation && <p className={cl.briefBlock}>{user.briefInformation}</p>}
                                                        </div>
                                                    }
                                                />
                                            </Card>
                                        </Link>
                                    ))}
                                    {!isUsersLoading && usersResults.length === 0 && <p className={cl.empty}>Список пуст. Измените параметры поиска людей.</p>}
                                </div>
                            </div>
                        )}

                        {/* ================= КОМПОНЕНТ ПОИСКА СООБЩЕСТВ ================= */}
                        {searchMode === "communities" && (
                            <div className={cl.searchSection}>
                                <form onSubmit={handleCommunitiesSubmit} className={cl.searchForm}>
                                    <h3 className={cl.sectionTitle}>Настройки фильтрации сообществ</h3>
                                    <Row gutter={[16, 16]}>
                                        <Col xs={24} sm={12} md={8}>
                                            <Input
                                                placeholder="Название сообщества"
                                                value={communityFilters.name}
                                                onChange={e => setCommunityFilters({ ...communityFilters, name: e.target.value })}
                                            />
                                        </Col>
                                        <Col xs={24} sm={12} md={8}>
                                            <Select
                                                mode="multiple" // Позволяет выбрать несколько категорий
                                                showSearch // Включает текстовый поиск прямо внутри селекта
                                                allowClear
                                                style={{ width: '100%' }}
                                                placeholder="Поиск категорий по названию..."
                                                // Связываем со значением в стейте (это будет массив ID, например [1, 2])
                                                value={communityFilters.communityCategoryIds}
                                                // Когда пользователь выбирает категорию, Ant Design передает массив выбранных values (id)
                                                onChange={selectedIds => setCommunityFilters({ ...communityFilters, communityCategoryIds: selectedIds })}
                                                // Логика фильтрации: ищем совпадение текста, введенного пользователем, с названием категории
                                                filterOption={(input, option) =>
                                                    option.children.toLowerCase().indexOf(input.toLowerCase()) >= 0
                                                }
                                            >
                                                {/* Рендерим опции: для пользователя показываем name, а в value прячем id */}
                                                {communitiesCategories.map(cat => (
                                                    <Option key={cat.id} value={cat.id}>
                                                        {cat.name}
                                                    </Option>
                                                ))}
                                            </Select>
                                        </Col>
                                        <Col xs={24} sm={12} md={8}>
                                            <Input
                                                placeholder="Имя создателя"
                                                value={communityFilters.managerName}
                                                onChange={e => setCommunityFilters({ ...communityFilters, managerName: e.target.value })}
                                            />
                                        </Col>
                                        <Col xs={24}>
                                            <Input
                                                placeholder="Описание сообщества"
                                                value={communityFilters.briefInformationSubstring}
                                                onChange={e => setCommunityFilters({ ...communityFilters, briefInformationSubstring: e.target.value })}
                                            />
                                        </Col>
                                    </Row>
                                    <Button type="primary" htmlType="submit" icon={<SearchOutlined />} loading={isCommunitiesLoading} className={cl.submitBtn}>
                                        Выполнить поиск
                                    </Button>
                                </form>

                                {communitiesError && <p className={cl.error}>Ошибка: {communitiesError}</p>}

                                {/* Вывод списка сообществ */}
                                <div className={cl.resultsContainer}>
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
                                    {!isCommunitiesLoading && communitiesResults.length === 0 && <p className={cl.empty}>Список пуст. Измените параметры поиска сообществ.</p>}
                                </div>
                            </div>
                        )}

                    </div>
                </ContentRuW>
            </Layout>
        </Layout>
    );
}

export default SearchPage;