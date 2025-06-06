import React from "react";
import cl from "./MyModal.module.css"

const MyModal = ({ children, visible, setVisible }) => {

    const rootClasses = [cl.myModal]
    if (visible) {
        rootClasses.push(cl.active)
    }

    return (
        <div className={rootClasses.join(' ')} onClick={(e) => {
            const myModalContentElem = document.querySelector(`.${cl.myModalContent}`)
            if (e.target !== myModalContentElem && !myModalContentElem.contains(e.target)) {
                setVisible(false)
            }
        }}>
            <div className={cl.myModalContent}>
                {children}
            </div>
        </div>
    );
};

export default MyModal;