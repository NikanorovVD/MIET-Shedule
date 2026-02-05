import "./SchedulePair.css"

export default function SchedulePair({ order, name, teacher, time, auditorium, ...props }) {
    return (
        <table {...props} className="schedule-pair">
            <tbody>
                <tr>
                    <td className="schedule-order">{order}</td>
                    <td className="schedule-name">{name}
                        <div>
                            <p className="schedule-teacher">{teacher}</p>
                            <p className="schedule-auditorium">{auditorium}</p>
                            <p className="schedule-time">{time.start.slice(0, 5)}-{time.end.slice(0, 5)}</p>
                        </div>
                    </td>
                </tr>
            </tbody>
        </table>
    )
}