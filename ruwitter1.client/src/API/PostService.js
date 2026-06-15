// import axios from "axios";

export default class PostService {
    /*
    static async getAll(limit = 10, page = 1) {
        const response = await axios.get('https://jsonplaceholder.typicode.com/posts', {
            params: {
                _limit: limit,
                _page: page
            }
        });
        return response;
    }

    static async getById(id) {
        const response = await axios.get('https://jsonplaceholder.typicode.com/posts/' + id);
        return response;
    }

    static async getCommentsByPostId(id) {
        const response = await axios.get(`https://jsonplaceholder.typicode.com/posts/${id}/comments`);
        return response;
    };
    */

    static async getPosts() {

        const posts = fetch(`/api/Post`, {
            method: "GET"
        }).then((response) => response.json());
        // console.log(posts);
        return posts;
    }

    static async getPersonalDataById(id) {
        const personData = fetch(`/api/DefaultUser/${id}`, {
            method: "GET"
        }).then((response) => response.json());
        // console.log(personData);
        return personData;
    }

    static async getCommunityById(id) {
        if (id === null) return;
        console.log(id);
        const community = fetch(`/api/Communities/${id}`, {
            method: "GET"
        }).then((response) => response.json());
        // console.log(personData);
        return community;
    }

    static async getCurrentUserCommunities() {
        const community = fetch(`/api/Communities/currentusercomm`, {
            method: "GET"
        }).then((response) => response.json());
        // console.log(personData);
        return community;
    }

    static async getCountOfPosts() {

        const postsCount = fetch('/api/Post/count', {
            method: "GET"
        }).then((response) => response.json());
        // console.log(posts);
        return postsCount;
    }


    static async getAvatarById(avatarId) {
        console.log(avatarId);
        try {
            const avatar = fetch(`/api/MediaFile/${avatarId}`, {
                method: "GET"
            }).then((response) => response.json());
            // console.log(posts);
            return avatar;

        } catch (err) {
            console.log(err.name);
            console.log(err.message);
            return undefined;
        }
        
    }

    static async isSetLikeByCurrentUser(postId, commentId = undefined) {
        const formData = new FormData();
        formData.append('PostId', postId);
        if (commentId !== undefined)
        {
            formData.append('CommentId', commentId);
        }
        console.log(formData);
        try {
            const response = await fetch(`api/Post/issetlike`, {
                method: 'POST',

                body: formData

            });
            return await response.json();
        } catch (error) {
            console.error('Error sending message with file:', error);
            throw error;
        }
    }

    static async setLikeByCommunity(id){
        try {
            const response = await fetch(`api/Post/community/like/${id}`, {
                method: 'POST',
            });
            console.log(response);
            return response;
        } catch (error) {
            console.error('Error sending message with file:', error);
            throw error;
        }
    }

    static async deleteLikeByCommunity(id) {
        try {
            const response = await fetch(`api/Post/delete/community/like/${id}`, {
                method: 'POST',
            });
            console.log(response);
            return response;
        } catch (error) {
            console.error('Error sending message with file:', error);
            throw error;
        }
    }

    static async setLike(id) {
        console.log(id);
        try {
            const response = await fetch(`api/Post/like/${id}`, {
                method: 'POST',
            });
            console.log(response);
            return response;
        } catch (error) {
            console.error('Error sending message with file:', error);
            throw error;
        }
    }

    static async deleteLike(id) {
        try {
            const response = await fetch(`api/Post/delete/like/${id}`, {
                method: 'POST',
            });
            console.log(response);
            return response;
        } catch (error) {
            console.error('Error sending message with file:', error);
            throw error;
        }
    }

    static async setCommentLikeByCommunity(postId, commentId) {
        try {
            const response = await fetch(`api/Post/community/like/${postId}/${commentId}`, {
                method: 'POST',
            });
            console.log(response);
            return response;
        } catch (error) {
            console.error('Error sending message with file:', error);
            throw error;
        }
    }

    static async deleteCommentLikeByCommunity(postId, commentId) {
        try {
            const response = await fetch(`api/Post/delete/community/like/${postId}/${commentId}`, {
                method: 'POST',
            });
            console.log(response);
            return response;
        } catch (error) {
            console.error('Error sending message with file:', error);
            throw error;
        }
    }

    static async setCommentLike(postId, commentId) {
        try {
            const response = await fetch(`api/Post/like/${postId}/${commentId}`, {
                method: 'POST',
            });
            console.log(response);
            return response;
        } catch (error) {
            console.error('Error sending message with file:', error);
            throw error;
        }
    }

    static async deleteCommentLike(postId, commentId) {
        try {
            const response = await fetch(`api/Post/delete/like/${postId}/${commentId}`, {
                method: 'POST',
            });
            console.log(response);
            return response;
        } catch (error) {
            console.error('Error sending message with file:', error);
            throw error;
        }
    }

    static async getPostById(postId) {

        const post = fetch(`/api/Post/post/${postId}`, {
            method: "GET"
        }).then((response) => response.json());
        // console.log(posts);
        return post;
    }

    static async getCurrentUserPosts() {

        const posts = fetch(`/api/Post/currentuserposts`, {
            method: "GET"
        }).then((response) => response.json());
        // console.log(posts);
        return posts;
    }

    static async getPostsByUserId(someUserId) {

        const posts = fetch(`/api/Post/userposts/${someUserId}`, {
            method: "GET"
        }).then((response) => response.json());
        // console.log(posts);
        return posts;
    }

    static async getPostsByCommunityId(id) {

        const posts = fetch(`/api/Post/community/${id}`, {
            method: "GET"
        }).then((response) => response.json());
        // console.log(posts);
        return posts;
    }

    static async IsCurrentUserCommunityManager(id) {
        const isOwner = fetch(`/api/Communities/isusercomm/${id}`, {
            method: "GET"
        }).then((response) => response.json());

        return isOwner;
    }

    static async getCommentsByPostId(postId) {

        const comments = fetch(`/api/Comment/post/${postId}`, {
            method: "GET"
        }).then((response) => response.json());
        // console.log(posts);
        return comments;
    }

    // static async getCurrUser() {
    //     const user = fetch('/api/DefaultUser/user', {
    //         method: "GET"
    //     }).then((response) => response.text());
    //     console.log(posts);
    //     return user;
    // }

    static async getCurrUserPersonalData() {
        const user = fetch('/api/DefaultUser/currentuserpersonaldata', {
            method: "GET"
        }).then((response) => response.json());
        // console.log(posts);
        return user;
    }

    static async getUserPersonalDataById(id) {
        const user = fetch(`/api/DefaultUser/${id}`, {
            method: "GET"
        }).then((response) => response.json());
        // console.log(posts);
        return user;
    }

    static async getChats() {
        const chats = fetch('/api/Chat', {
            method: "GET"
        }).then((response) => response.json());
        // console.log(posts);
        return chats;
    }


    static async getCommunititesCategories() {
        const categories = fetch('api/Communities/categories', {
            method: "GET"
        }).then((response) => response.json());
        // console.log(posts);
        return categories;
    }

    

    static async sendMessageWithFile(chatId, content, file){
        const formData = new FormData();
        formData.append('chatId', chatId);
        formData.append('content', content);
        if (file) {
            formData.append('files', file);
        }
        for (const [key, value] of formData.entries()) {
            console.log(`${key}: ${value}`);
        }
        try {
            const response = await fetch(`/api/Message`, {
                method: 'POST',
                
                body: formData

            });
            return response;
        } catch (error) {
            console.error('Error sending message with file:', error);
            throw error;
        }
    };

    static async deleteMessageById(messageId) {
        try {
            const response = await fetch(`/api/Message/${messageId}`, {
                method: 'DELETE'
            });
            return response;
        } catch (error) {
            console.error('Error deleting message:', error);
            throw error;
        }
    }

    static async createComment(postId, body, files) {
        const formData = new FormData();
        formData.append('body', body);
        formData.append('postId', postId);
        for (let i = 0; i < files.length; i++) {
            formData.append('formFiles', files[i]);
        }
        console.log(formData);
        try {
            const response = await fetch(`api/Comment/${postId}`, {
                method: 'POST',

                body: formData

            });
            return response;
        } catch (error) {
            console.error('Error sending message with file:', error);
            throw error;
        }
    }

    static async createPost(body, files) {
        const formData = new FormData();
        formData.append('body', body);
        for (let i = 0; i < files.length; i++) {
            formData.append('formFiles', files[i]);
        }
        console.log(formData);
        try {
            const response = await fetch(`api/Post`, {
                method: 'POST',

                body: formData

            });
            return response;
        } catch (error) {
            console.error('Error sending message with file:', error);
            throw error;
        }
    }

    static async createCommunityPost(body, communityId, files) {
        const formData = new FormData();
        formData.append('body', body);
        for (let i = 0; i < files.length; i++) {
            formData.append('formFiles', files[i]);
        }
        console.log(formData);
        try {
            const response = await fetch(`api/Post/community/${communityId}`, {
                method: 'POST',

                body: formData

            });
            return response;
        } catch (error) {
            console.error('Error sending message with file:', error);
            throw error;
        }
    }

    static async updatePost(postId, body, files) {
        const formData = new FormData();
        formData.append('postId', postId);
        formData.append('body', body);
        for (let i = 0; i < files.length; i++) {
            formData.append('formFiles', files[i]);
        }
        console.log(formData);
        try {
            const response = await fetch(`api/Post/${postId}`, {
                method: 'PUT',

                body: formData

            });
            console.log(response);
            return response;
        } catch (error) {
            console.error('Error sending message with file:', error);
            throw error;
        }
    }

    static async deletePostById(postId) {
        const formData = new FormData();
        console.log(formData);
        try {
            const response = await fetch(`api/Post/${postId}`, {
                method: 'DELETE'

            });
            console.log(response);
            return response;
        } catch (error) {
            console.error('Error sending message with file:', error);
            throw error;
        }
    }

    static async updateCurrentUserProfile(newProfile, avatar) {
        const formData = new FormData();
        formData.append('PhoneNumber', newProfile.phoneNumber);
        formData.append('Nickname', newProfile.nickname);
        formData.append('Age', newProfile.age);
        formData.append('BriefInformation', newProfile.briefInformation);
        formData.append('City', newProfile.city);
        formData.append('Interests', newProfile.interests);
        formData.append('Avatar', avatar);


        console.log(formData);
        try {
            const response = await fetch(`api/DefaultUser/personaldata`, {
                method: 'PUT',

                body: formData

            });
            return response;
        } catch (error) {
            console.error('Error sending message with file:', error);
            throw error;
        }
    }

    static async createCommunityProfile(newProfile, avatar) {
        const formData = new FormData();
        formData.append('name', newProfile.name);
        formData.append('categoryId', newProfile.communityCategoryId);
        formData.append('briefInformation', newProfile.briefInformation);
        formData.append('avatars', avatar);
        for (const [key, value] of formData.entries()) {
            console.log(`${key}: ${value}`);
        }

        console.log(formData);
        try {
            const response = await fetch(`/api/Communities`, {
                method: 'POST',

                body: formData

            });
            return response;
        } catch (error) {
            console.error('Error sending message with file:', error);
            throw error;
        }
    }


    static async updateCommunityProfile(newProfile, avatar, communityId) {
        const formData = new FormData();
        formData.append('name', newProfile.name);
        formData.append('categoryId', newProfile.communityCategoryId);
        formData.append('briefInformation', newProfile.briefInformation);
        formData.append('avatars', avatar);
        for (const [key, value] of formData.entries()) {
            console.log(`${key}: ${value}`);
        }

        console.log(formData);
        try {
            const response = await fetch(`/api/Communities/updatebrief/${communityId}`, {
                method: 'POST',

                body: formData

            });
            return response;
        } catch (error) {
            console.error('Error sending message with file:', error);
            throw error;
        }
    }

    // Внутри класса PostService в файле PostService.js
    static async getChatById(id) {
        const chat = fetch(`/api/Chat/${id}`, {
            method: "GET"
        }).then((response) => response.json());
        // console.log(chat);
        return chat;
    }

    static async getChatByUserId(someUserId) {
        const chatId = fetch(`/api/Chat/user/${someUserId}`, {
            method: "GET"
        }).then((response) => response.json());
        // console.log(chat);
        return chatId;
        
    }

    static async getMessagesByChatId(chatId) {
        const messages = fetch(`/api/Message/${chatId}`, {
            method: "GET"
        }).then((response) => response.json());
        // console.log(messages);
        return messages;
    }

    static async getChatsByCurrentUser() {
        return fetch(`/api/Chat`, {
            method: "GET"
        }).then((response) => response.json());
    }

    // Хелпер для получения медиафайлов конкретного сообщения
    static async getMediaFilesByMessageId(messageId) {
        try {
            return await fetch(`/api/MediaFile/message/${messageId}`, {
                method: "GET"
            }).then((response) => response.json());
        } catch (e) {
            console.error("Ошибка получения медиафайлов для сообщения:", messageId, e);
            return [];
        }
    }

    // Полная сборка данных для списка чатов (чтобы не перегружать компонент циклами)
    static async getFullChatsList() {
        const currUser = await PostService.getCurrUserPersonalData();
        console.log(currUser);
        const rawChats = await this.getChatsByCurrentUser();
        console.log(rawChats);
        const enrichedChats = [];

        for (let chat of rawChats) {
            try {
                // Получаем сообщения чата
                const messages = await this.getMessagesByChatId(chat.id);
                console.log(messages);

                // Определяем, кто собеседник
                const opponentId = chat.users[0].id === currUser.userId ? chat.users[1].id : chat.users[0].id;
                console.log(opponentId);

                // Получаем данные собеседника (используем твой метод getPersonalDataById)
                const opponentData = await this.getPersonalDataById(opponentId);

                // Подтягиваем аватар (используем твой метод getAvatarById)
                if (opponentData && opponentData.avatarId) {
                    opponentData.avatar = await this.getAvatarById(opponentData.avatarId);
                }

                chat.user = opponentData;
                chat.messages = messages; // Свежие сообщения обычно идут первыми на бэкенде
                enrichedChats.push(chat);
            } catch (error) {
                console.error(`Ошибка сборки чата ${chat.id}:`, error);
                enrichedChats.push(chat); // Добавляем хотя бы сырые данные, чтобы приложение не падало
            }
        }
        return enrichedChats;
    }

    static async searchUsers(filters) {
        const formData = new FormData();
        // Названия ключей должны точно совпадать с параметрами в сигнатуре твоего [HttpPost] контроллера
        formData.append('phoneNumber', filters.phoneNumber || "");
        formData.append('nickname', filters.nickname || "");
        formData.append('ageFrom', filters.ageFrom || 1);
        formData.append('ageTo', filters.ageTo || 100);
        formData.append('city', filters.city || "");
        formData.append('interests', filters.interests || "");
        for (const [key, value] of formData.entries()) {
            console.log(`${key}: ${value}`);
        }
        try {
            // Теперь эндпоинт официально принимает POST-запросы
            const response = await fetch('api/DefaultUser/users-search', {
                method: 'POST',
                body: formData
            });
            return await response.json();
        } catch (error) {
            console.error('Error searching users:', error);
            throw error;
        }
    }

    static async searchCommunities(filters) {
        const formData = new FormData();
        formData.append('name', filters.name || "");
        formData.append('briefInformationSubstring', filters.briefInformationSubstring || "");
        formData.append('managerName', filters.managerName || "");

        // Передаем массив ID категорий для List<int> на бэкенде
        if (filters.communityCategoryIds && filters.communityCategoryIds.length > 0) {
            filters.communityCategoryIds.forEach(id => {
                formData.append('communityCategoryIds', id);
            });
        }
        for (const [key, value] of formData.entries()) {
            console.log(`${key}: ${value}`);
        }
        try {
            const response = await fetch('api/Communities/search', {
                method: 'POST',
                body: formData
            });
            return await response.json();
        } catch (error) {
            console.error('Error searching communities:', error);
            throw error;
        }
    }

    static async searchCommunityPosts(filters) {
        const formData = new FormData();
        formData.append('postSubString', filters.textSubstring || "");
        formData.append('communityNameSubString', filters.communityName || "");
        formData.append('dateTimeFrom', filters.dateFrom || "");
        formData.append('dateTimeTo', filters.dateTo || "");

        // Передаем массив ID категорий
        if (filters.categoryIds && filters.categoryIds.length > 0) {
            filters.categoryIds.forEach(id => {
                formData.append('communityCategoryIds', id);
            });
        }

        try {
            const response = await fetch('/api/Post/search', {
                method: 'POST',
                body: formData
            });

            if (!response.ok) {
                const errorText = await response.text();
                console.error(`Ошибка поиска записей ${response.status}:`, errorText);
                throw new Error(errorText);
            }

            return await response.json();
        } catch (error) {
            console.error('Error searching community posts:', error);
            throw error;
        }
    }

    static async createChat(acceptorUserId) {
        const formData = new FormData();
        formData.append('acceptorUserId', acceptorUserId);
        for (const [key, value] of formData.entries()) {
            console.log(`${key}: ${value}`);
        }
        try {
            const response = await fetch('api/Chat', {
                method: 'POST',
                body: formData
            });
            return await response.json();
        } catch (error) {
            console.error('Error searching communities:', error);
            throw error;
        }
    }


}