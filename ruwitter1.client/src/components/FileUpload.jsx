// components/FileUpload.jsx
import React, { useState } from 'react';
import axios from 'axios';

const FileUpload = () => {
    const [file, setFile] = useState(null);
    const [uploadProgress, setUploadProgress] = useState(0);
    const [uploadStatus, setUploadStatus] = useState('');

    const handleFileChange = (e) => {
        setFile(e.target.files[0]);
    };

    const handleUpload = async () => {
        if (!file) {
            alert('Please select a file first!');
            return;
        }

        const formData = new FormData();
        formData.append('file', file);

        try {
            setUploadStatus('Uploading...');
            
            const response = await axios.post('https://127.0.0.1/api/MediaFile', formData, {
                headers: {
                    'Content-Type': 'multipart/form-data'
                },
                onUploadProgress: (progressEvent) => {
                    const percentCompleted = Math.round((progressEvent.loaded * 100) / progressEvent.total);
                    setUploadProgress(percentCompleted);
                }
            });

            setUploadStatus('Upload successful!');
            console.log('File uploaded:', response.data);
        } catch (error) {
            console.error('Error uploading file:', error);
            setUploadStatus('Upload failed.');
        }
    };

    return (
        <div>
            <h2>Upload File</h2>
            <input type="file" onChange={handleFileChange} />
            <button onClick={handleUpload}>Upload</button>
            
            {uploadProgress > 0 && (
                <div>
                    <progress value={uploadProgress} max="100" />
                    <span>{uploadProgress}%</span>
                </div>
            )}
            
            {uploadStatus && <p>{uploadStatus}</p>}
        </div>
    );
};

export default FileUpload;