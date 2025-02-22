import useDate from "../hooks/useDate";
import DatePicker from "./Button/DatePicker";
import SheduleCouple from "./SheduleCouple";
import "./SheduleSection.css"
import { useCallback, useEffect, useState } from "react";


export default function SheduleSection() {

    const url = new URL(window.location.href);
    const urlParams = new URLSearchParams(url.search)
    const defaultGroup = urlParams.get("group") ?? ""
    const defaultIgnored = urlParams.get("ignore") ?? ""

    const [group, setGroup] = useState(defaultGroup)
    const [groupsList, setGroupsList] = useState()
    const [shedule, setShedule] = useState()
    const { date, setDate, minusDay, addDay } = useDate()
    const [ignored, setIgnored] = useState(defaultIgnored)
    const [invalidGroup, setInvalidGroup] = useState(false)

    const fetchGroupsList = useCallback(async () => {
        const groupsListResponse = await fetch("Groups")
        if (groupsListResponse.status == 200) {
            const groups = await groupsListResponse.json()
            setGroupsList(groups)
        }
    }, [])

    const fetchShedule = useCallback(async () => {
        if (group.length != 0) {
            let query = `Shedule/${group}?dateString=${(new Date(date)).toLocaleDateString("en-GB")}`
            if (ignored.length != 0) query += `&ignored=${ignored}`
            const sheduleResponse = await fetch(query)
            if (sheduleResponse.status == 200) {
                const sheduleRecords = await sheduleResponse.json()
                setShedule(sheduleRecords)
            }
        }
    }, [group, date, ignored])

    useEffect(() => {
        fetchGroupsList()
    }, [])

    useEffect(() => {
        if (groupsList != undefined && groupsList.includes(group)) {
            fetchShedule()
            setInvalidGroup(false)
        }
        else {
            setShedule(undefined)
            if (group.length != 0) {
                setInvalidGroup(true)
            }
        }
    }, [group, date, groupsList, ignored])

    function keyExtractor(couple) {
        return `${couple.name}-${couple.date}-${couple.teacher}-${couple.order}-${couple.auditorium}-${couple.group}`;
    }

    return (
        <section className="shedule_section">
            {groupsList != undefined &&
                <>
                    <div>
                        <input className={invalidGroup ? "error-input" : ""} type="text" list="groups" value={group} onChange={(event) => setGroup(event.target.value)} />
                        <datalist id="groups">
                            {groupsList.map(g => <option key={g}>{g}</option>)}
                        </datalist>
                    </div>
                    <DatePicker date={date} minusDay={() => minusDay()} addDay={() => addDay()} setDate={setDate} />
                </>

            }

            {shedule != undefined &&
                shedule.map(c =>
                    <SheduleCouple key={keyExtractor(c)} {...c}></SheduleCouple>
                )
            }

            {
                shedule != undefined && shedule.length == 0 &&
                <h2>Нет занятий</h2>
            }

            {
                ignored.length != 0 &&
                <p>Ignore: {ignored}</p>
            }
        </section>
    )
}