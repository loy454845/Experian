import React, { Fragment, useState } from 'react';
import axios from 'axios';
import ProccessDataGrid from './ProcessDataGrid';

function FileUpload() {
    const [selectedFile, setSelectedFile] = useState();
    const [isFileUploaded, setIsFileUploaded] = useState(false);
    const [processedFileData, setProcessedFileData] = useState();

    const fileUploadHandler = (event) => {
        setSelectedFile(event.target.files[0]);
        setIsFileUploaded(true);
    }

    const handleSubmission = () => {
        //Fetch API and post the file as a parameter
        const formData = new FormData();

        formData.append('File', selectedFile);

        axios.post(`https://localhost:5001/Financial`, formData)
            .then(response => {
                setProcessedFileData(response);
            })
            .catch(function (error) {
                console.log(error);
            })
    }

    return (
        <Fragment>
            <h1>Experian Tech Test</h1>
            <div>
                <p> Please upload Finance file .</p>
                <div>
                    <input type="file" name="csvFile" onChange={fileUploadHandler} />
                </div>
                <div>
                    <button onClick={handleSubmission}>Upload</button>
                </div>
                {(!isFileUploaded || (selectedFile.type !== "text/csv") ?
                    (
                        <div>
                            <p>Choose correct Finance file and File type should be csv format.</p>
                        </div>
                    ) : (
                        <ProccessDataGrid records={processedFileData}></ProccessDataGrid>
                    )
                )}
            </div>
        </Fragment>
    );
};

export default FileUpload;