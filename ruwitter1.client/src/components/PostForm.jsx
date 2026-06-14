import React, { useState, useRef } from "react";
import MyButton from "./UI/button/MyButton";
import MyInput from "./UI/input/MyInput";
import clform from "./UI/CommentFormStyle/CommentForm.module.css";
import plform from "./UI/PostFormStyle/PostFormStyle.module.css";
import { PaperClipOutlined, CheckOutlined } from '@ant-design/icons';
import { Link } from 'react-router-dom';

const PostForm = ({ create, filledBody }) => {
    const [post, setPost] = useState({ body: '', files: [] });

    const fileInputRef = useRef(null);

    const addNewPost = (e) => {
        e.preventDefault()
        const newPost = {
            ...post
        }
        console.log(newPost);
        if (newPost.body === '') {
            return;
        }

        if (newPost.files.length > 5) {
            setPost({ ...post, files: [] });
            return;
        }
        create(newPost)
        setPost({ body: '', files: [] })
    }

    const handleImageClick = () => {
        fileInputRef.current.click();
    };


    return (
        <div className={plform.post_form_container }>
            <form className={plform.post_form}>
                {/*Управляемый компонент*/}
                <div className={plform.post_inputs}>
                    <textarea
                        value={post.body}
                        name='body'
                        onChange={e => setPost({ ...post, body: e.target.value })}
                        type="text"
                        placeholder="Напишите что-нибудь..."
                        maxLength="10000"
                        // rows="10"
                        // cols="50"
                        className={plform.post_form_body}
                    ></textarea>
                    <label htmlFor='files' className={plform.post_label} style={{ color: '#888888' }}>Не больше 5 файлов</label>
                    <MyInput
                        style={{ display: 'none' }}
                        name='files'
                        type="file"
                        onChange={e => {
                            // Превращаем FileList в стандартный массив JS
                            const chosenFiles = Array.from(e.target.files);

                            setPost({
                                ...post,
                                // Объединяем старые файлы с новыми
                                files: [...post.files, ...chosenFiles]
                            });
                        }}

                        multiple
                        accept="image/*"
                        ref={fileInputRef}
                    />
                    <PaperClipOutlined onClick={handleImageClick} style={{ fontSize: '40px' }} />
                </div>
                <div style={{ display: 'flex', width: '520px', justifyContent: 'space-between' }}>
                    {filledBody && <Link to={'/profile'} ><MyButton style={{ backgroundColor: '#FFFFFF', color: '#1DA1F2' }}>Отменить</MyButton></Link>}
                    <MyButton onClick={addNewPost}>Опубликовать</MyButton>
                </div>

            </form>
        </div>
        
    );

};

export default PostForm;