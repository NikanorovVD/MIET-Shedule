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
        <table {...props} className="shedule-pair">
            <tbody>
                <tr>
                    <td className="shedule-order">{`${day}.${month}`}</td>
                    <td className="shedule-name">{name}
                        <div>
                            <p className="shedule-teacher">{teacherName}</p>
                            <p className="shedule-auditorium">{auditorium}</p>
                            <p className="shedule-rest-time">{getDaysText()}</p>
                        </div>
                    </td>
                </tr>
            </tbody>
        </table>
    )
}