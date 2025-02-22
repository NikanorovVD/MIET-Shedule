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
            <h3>Загрузить расписание МИЭТ</h3>
            <button
                className='download_button'
                onClick={() => downloadFile('Export/Adapted', 'MIET-Shedule-adapted.json')}>
                В адаптированном формате
            </button>
            <button
                className='download_button'
                onClick={() => downloadFile('Export/Origin', 'MIET-Shedule-origin.json')}>
                В исходном формате MIET-API
            </button>
        </section>
    )
}