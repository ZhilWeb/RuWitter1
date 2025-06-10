import React from 'react';
import { Layout, Menu } from 'antd';
import { UserOutlined, HomeOutlined, SearchOutlined, FormOutlined, PicRightOutlined, TeamOutlined, MessageOutlined, LogoutOutlined } from '@ant-design/icons';
import cl from './SidebarRuW.module.css';
import LogoutLink from "../../components/LogoutLink";

const { Header, Content, Footer, Sider } = Layout;

/*
const sidebarItems = [
    <a href="#">
        <FormOutlined />
        <label>�������� ����� �������</label>
    </a>,
    <a href="#">
        <PicRightOutlined />
        <label>�������</label>
    </a>,
    <a href="#">
        <TeamOutlined />
        <label>������</label>
    </a>,
    <a href="#">
        <LogoutOutlined />
        <label>�����</label>
    </a>
]
*/


const SidebarRuW = ({isActive }) => {

    return (
        <div className={`${cl.sidebar} ${ isActive ? cl.active : null }`}>
            <div className={cl.menu} /*style={{ height: '100%', display: 'flex', flexDirection: 'column', marginTop: '200px' }}*/>
                <div className={cl.mobile_sidebar }>
                    <UserOutlined className={cl.icons} />
                    <a href="#">Профиль</a>
                </div>
                <div>
                    <PicRightOutlined className={cl.icons} />
                    <a href="/">Новости</a>
                </div>
                <div>
                    <FormOutlined className={cl.icons } />
                    <a href="#">Добавить новую историю</a>
                </div>
                <div>
                    <SearchOutlined className={cl.icons} />
                    <a href="#">Поиск</a>
                </div>
                <div>
                    <TeamOutlined className={cl.icons} />
                    <a href="#">Друзья</a>
                </div>
                <div>
                    <MessageOutlined className={cl.icons} />
                    <a href="/chats">Сообщения</a>
                </div>
                <div>
                    <LogoutOutlined className={cl.icons} />
                    <span><LogoutLink>Выйти</LogoutLink></span>
                </div>
            </div>
            <Footer style={{ backgroundColor: '#FFF', marginTop: 'auto' }}>
                RuWitter ©{new Date().getFullYear()} Created by Жилин П. И.
            </Footer>
        </div>
    );
};

export default SidebarRuW;