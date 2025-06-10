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

    static async getPosts(lastPostId) {

        const posts = fetch(`/api/Post/${lastPostId}`, {
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

    static async getCountOfPosts() {

        const postsCount = fetch('/api/Post/count', {
            method: "GET"
        }).then((response) => response.json());
        // console.log(posts);
        return postsCount;
    }


    static async getAvatarById(avatarId) {
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

    static async getPostById(postId) {

        const comments = fetch(`/api/Post/post/${postId}`, {
            method: "GET"
        }).then((response) => response.json());
        // console.log(posts);
        return comments;
    }

    static async getCommentsByPostId(postId) {

        const comments = fetch(`/api/Comment/post/${postId}`, {
            method: "GET"
        }).then((response) => response.json());
        // console.log(posts);
        return comments;
    }

    static async getCurrUser() {
        const user = fetch('/api/DefaultUser/user', {
            method: "GET"
        }).then((response) => response.text());
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

    static async getChatById(chatId) {
        const chat = fetch(`/api/Chat/${chatId}`, {
            method: "GET"
        }).then((response) => response.json());
        // console.log(posts);
        return chat;
    }

    static async getMessagesByChatId(chatId) {
        const messages = fetch(`/api/Message/${chatId}`, {
            method: "GET"
        }).then((response) => response.json());
        // console.log(posts);
        return messages;
    }

    static async sendMessageWithFile(content, file, chatId){
        const formData = new FormData();
        formData.append('content', content);
        formData.append('chatId', chatId);
        if (file) {
            formData.append('file', file);
        }
        console.log(formData);
        try {
            const response = await fetch(`api/Message/${chatId}`, {
                method: 'POST',
                
                body: formData

            });
            return response;
        } catch (error) {
            console.error('Error sending message with file:', error);
            throw error;
        }
    };
}