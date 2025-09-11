import "./ShedulePair.css"

export default function ShedulePair({ order, name, teacher, time, auditorium, ...props }) {
    return (
        <table {...props} className="shedule-pair">
            <tbody>
                <tr>
                    <td className="shedule-order">{order}</td>
                    <td className="shedule-name">{name}
                        <div>
                            <p className="shedule-teacher">{teacher}</p>
                            <p className="shedule-auditorium">{auditorium}</p>
                            <p className="shedule-time">{time.start.slice(0, 5)}-{time.end.slice(0, 5)}</p>
                        </div>
                    </td>
                </tr>
            </tbody>
        </table>
    )
}