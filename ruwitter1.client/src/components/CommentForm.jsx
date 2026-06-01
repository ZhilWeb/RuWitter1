import React, { useState, useRef } from "react";
import MyButton from "./UI/button/MyButton";
import MyInput from "./UI/input/MyInput";
import clform from "./UI/CommentFormStyle/CommentForm.module.css";
import { PaperClipOutlined, CheckOutlined } from '@ant-design/icons';

const CommentForm = ({ create }) => {

    const [comment, setComment] = useState({ body: '', files: [] });
    const fileInputRef = useRef(null);
    const buttonRef = useRef(null);

    const addNewComment = (e) => {
        e.preventDefault()
        const newComment = {
            ...comment
        }
        console.log(newComment);
        if (newComment.body === '') {
            return;
        }

        if (newComment.files.length > 2) {
            setComment({ ...comment, files: [] });
            return;
        }
        create(newComment)
        setComment({ body: '', files: [] })
    }

    const handleImageClick = () => {
        fileInputRef.current.click();
    };

    const handleButtonClick = () => {
        buttonRef.current.click();
    };

    return (
        <form className={ clform.comment_form }>
            {/*Управляемый компонент*/}

            <MyInput
                value={comment.body}
                name='body'
                onChange={e => setComment({ ...comment, body: e.target.value })}
                type="text"
                placeholder="Добавить комментарий"
                maxLength="10000"
            />
            <label htmlFor='files' className={clform.comment_label} style={{ display: 'none' }}>Не больше 2 файлов</label>
            <MyInput
                style={{ display: 'none' }}
                name='files'
                type="file"
                onChange={e => {
                    // Превращаем FileList в стандартный массив JS
                    const chosenFiles = Array.from(e.target.files);

                    setComment({
                        ...comment,
                        // Объединяем старые файлы с новыми
                        files: [...comment.files, ...chosenFiles]
                    });
                }}

                multiple
                accept="image/*"
                ref={fileInputRef}
            />
            <PaperClipOutlined onClick={handleImageClick} style={{ fontSize: '40px' }} />
            <MyButton onClick={addNewComment} ref={buttonRef} style={{ display: 'none' }}>Создать пост</MyButton>
            <CheckOutlined onClick={handleButtonClick} style={{ fontSize: '40px' }} />
        </form>
    );

};

export default CommentForm;