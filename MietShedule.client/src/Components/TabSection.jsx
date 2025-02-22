import Button from "./Button/Button"
import "./TabSection.css"

export default function TabSection({ activeTab, OnChange }) {
    const url = new URL(window.location.href)
    const ip = url.hostname.split(':')[0]
    return (
        <section className="tab-section">
            <Button active={activeTab == 'shedule'} onClick={() => OnChange('shedule')}>Расписание</Button>
            <Button active={activeTab == 'teacher'} onClick={() => OnChange('teacher')}>Поиск преподавателя</Button>
            <Button active={activeTab == 'export'} onClick={() => OnChange('export')}>Экспорт расписания</Button>
            <a href={`https://${ip}:7056/swagger/index.html`} target="_blank" rel="noopener noreferrer">
                <button className="button">API</button>
            </a>
        </section>
    )
}