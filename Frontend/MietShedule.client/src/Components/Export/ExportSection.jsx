import './ExportSection.css'

export default function ExportSection() {
    async function downloadFile(url, filename) {
        await fetch(url)
            .then(response => {
                response.blob().then(blob => {
                    let url = window.URL.createObjectURL(blob)
                    let a = document.createElement('a')
                    a.href = url
                    a.download = filename
                    a.click()
                })
            })
    }

    return (
        <section >
            <h3 className='export-header'>Загрузить расписание МИЭТ</h3>
            <button
                className='download-button'
                onClick={() => downloadFile('Export', 'MIET-Shedule.json')}>
                Скачать файл
            </button>
        </section>
    )
}