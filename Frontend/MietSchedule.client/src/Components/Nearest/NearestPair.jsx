import "./NearestPair.css"

export default function NearestPair({ targetDate, order, name, teacherName, auditorium, remainingDays, ...props }) {
    const parsedDate = new Date(targetDate)
    const zeroPad = (num, places) => String(num).padStart(places, '0')
    const day = zeroPad(parsedDate.getDate(), 2)
    const month = zeroPad(parsedDate.getMonth() + 1, 2)

    const getDaysText = () => {
        if (remainingDays === 0) return "Сегодня";
        if (remainingDays === 1) return "Завтра";
        return `через ${remainingDays} дн`;
    }

    return (
        <table {...props} className="schedule-pair">
            <tbody>
                <tr>
                    <td className="schedule-order">{`${day}.${month}`}</td>
                    <td className="schedule-name">{name}
                        <div>
                            <p className="schedule-teacher">{teacherName}</p>
                            <p className="schedule-auditorium">{auditorium}</p>
                            <p className="schedule-rest-time">{getDaysText()}</p>
                        </div>
                    </td>
                </tr>
            </tbody>
        </table>
    )
}