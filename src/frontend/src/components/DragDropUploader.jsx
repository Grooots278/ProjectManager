import { useDropzone } from 'react-dropzone';

export default function DragDropUpLoader({ files, onAddFiles, onRemoveFile }){
    const { getRootProps, getInputProps, isDragActive } = useDropzone({
        onDrop: (acceptedFiles) => {
            const newFiles = acceptedFiles.map(file => ({
                file,
                name: file.name,
                size: file.size,
                preview: URL.createObjectURL(file),
            }));
            onAddFiles(newFiles);
        },
    });

    return (
        <div>
            <div {...getRootProps()} style={{ border: '2px dashed #ccc', padding: '20px', textAlign: 'center', cursor: 'pointer' }}>
                <input {...getInputProps()} />
                {isDragActive ? (
                    <p>Move the file here...</p>
                ) : (
                    <p>Drag and drop the files here or click to select</p>
                )}
            </div>
            {files.length > 0 && (
                <ul>
                    {files.map((f, index) => {
                        <li key={index}>
                            {f.name} ({(f.size / 1024).toFixed(1)} KB)
                            <button type='button' onClick={() => onRemoveFile(index)}>Delete</button>
                        </li>
                    })}
                </ul>
            )}
        </div>
    );
}