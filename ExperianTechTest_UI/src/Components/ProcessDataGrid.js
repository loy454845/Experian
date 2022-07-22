function processedFileData(props) {
    // const { processedRecords } = props;

    const processedData = props.records;

    const load = (errorRecords) => {
        const inputdata = errorRecords;
         return (
            <div>
               {/* not implemented yet */}
            </div>
        )
    }

    if (processedData != undefined && processedData != null) {
        if (processedData.data != null) {
            return (<div>
                <h1>Total Record Count: {processedData.data.totalRecordCount}</h1>
                <h1>Valid Record Count: {processedData.data.validRecords}</h1>
                <h1>Invalid Record Count: {processedData.data.invalidRecords}</h1>
            </div>);
        }
    } else {
        return (<h2> No data to process</h2>);
    }
}

export default processedFileData;